#include "pch.h"
#include "AudioTrack.h"
#include <mfidl.h>
#include <mfapi.h>
#include <mfreadwrite.h>

#pragma comment(lib, "Mfplat.lib")
#pragma comment(lib, "Mfreadwrite.lib")
#pragma comment(lib, "Mfuuid.lib")

AudioTrack::AudioTrack(void)
{
	_ready = false;
}

AudioTrack::~AudioTrack(void)
{
}

void AudioTrack::Play()
{
	if (!_ready) return;

	XAUDIO2_BUFFER buff = {0};

	_voice->Stop();
	_voice->FlushSourceBuffers();

	buff.AudioBytes = _buffLen;
	buff.pAudioData = _data;
	buff.Flags = XAUDIO2_END_OF_STREAM;

	_voice->SubmitSourceBuffer(&buff);
	_voice->Start();
}

void AudioTrack::Stop()
{
	if (!_ready) return;

	_voice->Stop();
}

void AudioTrack::Load(wchar_t *path, IXAudio2 *engine)
{
	// TODO: Load audio
	Microsoft::WRL::ComPtr<IMFSourceReader> reader;
	if (FAILED(MFCreateSourceReaderFromURL(path, nullptr, &reader))) return;

	Microsoft::WRL::ComPtr<IMFMediaType> mediaType;
	if (FAILED(MFCreateMediaType(&mediaType))) return;

	if (FAILED(mediaType->SetGUID(MF_MT_MAJOR_TYPE, MFMediaType_Audio))) return;

	if (FAILED(mediaType->SetGUID(MF_MT_SUBTYPE, MFAudioFormat_PCM))) return;

	if (FAILED(reader->SetCurrentMediaType(MF_SOURCE_READER_FIRST_AUDIO_STREAM, 0, mediaType.Get()))) return;

	Microsoft::WRL::ComPtr<IMFMediaType> outputType;
	if (FAILED(reader->GetCurrentMediaType(MF_SOURCE_READER_FIRST_AUDIO_STREAM, &outputType))) return;

	UINT32 size = 0;
	WAVEFORMATEX *comFmt;
	WAVEFORMATEX fmt;
	if (FAILED(MFCreateWaveFormatExFromMFMediaType(outputType.Get(), &comFmt, &size))) return;
	CopyMemory(&fmt, comFmt, sizeof(WAVEFORMATEX));
	CoTaskMemFree(comFmt);

	PROPVARIANT propVariant;
	if (FAILED(reader->GetPresentationAttribute(MF_SOURCE_READER_MEDIASOURCE, MF_PD_DURATION, &propVariant))) return;

	LONGLONG duration = propVariant.uhVal.QuadPart;
	unsigned int maxStreamLenInBytes;

	double durationSeconds = (duration / static_cast<double>(10000 * 1000));
	maxStreamLenInBytes = static_cast<unsigned int>(durationSeconds * fmt.nAvgBytesPerSec);

	// Ensure it is a multiple of 4 bytes
	maxStreamLenInBytes = (maxStreamLenInBytes + 3) / 4 * 4;

	BYTE *fileData = new BYTE[maxStreamLenInBytes];

	Microsoft::WRL::ComPtr<IMFSample> sample;
	Microsoft::WRL::ComPtr<IMFMediaBuffer> buffer;
	DWORD flags = 0;

	int pos = 0;
	bool done = false;
	while (!done)
	{
		if (FAILED(reader->ReadSample(MF_SOURCE_READER_FIRST_AUDIO_STREAM, 0, nullptr, &flags, nullptr, &sample))) return;

		if (sample != nullptr)
		{
			if (FAILED(sample->ConvertToContiguousBuffer(&buffer))) return;

			BYTE *audioData = nullptr;
			DWORD sampleLen = 0;

			if (FAILED(buffer->Lock(&audioData, nullptr, &sampleLen))) return;

			for (DWORD i = 0; i < sampleLen; ++i)
				fileData[pos++] = audioData[i];
		}

		if (flags & MF_SOURCE_READERF_ENDOFSTREAM)
			done = true;
	}

	_data = new BYTE[(pos + 3) / 4 * 4];
	memcpy(fileData, _data, pos);
	delete fileData;

	if (SUCCEEDED(engine->CreateSourceVoice(&_voice, &fmt)))
		_ready = true;
}
