/* Copyright (c) 2017 Red-EyeX32
*
* This software is provided 'as-is', without any express or implied
* warranty. In no event will the authors be held liable for any damages arising from the use of this software.
*
* Permission is granted to anyone to use this software for any purpose,
* including commercial applications*, and to alter it and redistribute it
* freely, subject to the following restrictions:
*
* 1. The origin of this software must not be misrepresented; you must not
*    claim that you wrote the original software. If you use this software
*    in a product, an acknowledge in the product documentation is required.
*
* 2. Altered source versions must be plainly marked as such, and must not
*    be misrepresented as being the original software.
*
* 3. This notice may not be removed or altered from any source distribution.
*
* *Contact must be made to discuss permission and terms.
*/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <windows.h>

#include "types.h"
#include "aes.h"
#include "sha1.h"

#define TITLE_METADATA_LINK_MAX_LEN    256
#define TITLE_METADATA_HMAC_DIGEST_LEN 20
#define TITLE_METADATA_HMAC_KEY_LEN    64

static const char tm_key[27] = "OrbisTitleMetadataDatabase";

static u8 aes_iv[16] = {
	0xb4, 0xf4, 0x11, 0x92, 0xb6, 0xe1, 0x7c, 0x86,
	0xff, 0x2f, 0xc5, 0xed, 0xc2, 0x5c, 0x95, 0x07
};

static u8 aes_key[32] = {
	0x48, 0xe2, 0xc5, 0xa1, 0x7d, 0xb5, 0x23, 0xcb, 0x7a, 0xef, 0xfa, 0xa6, 0x2a, 0x0d, 0xc2, 0xa2,
	0x98, 0x97, 0xdf, 0x0c, 0x88, 0x24, 0xb0, 0xd4, 0x11, 0x11, 0xac, 0xff, 0x87, 0xec, 0xb0, 0x51
};

u8 tmdb_json_hmac_key[64] = {
	0x83, 0x1c, 0x47, 0x55, 0x6f, 0x18, 0xd0, 0x9b, 0x51, 0xbf, 0x4a, 0x57, 0x9c, 0x11, 0x3c, 0x2a,
	0x30, 0x02, 0x3b, 0x0e, 0x88, 0xba, 0x56, 0x9d, 0x4a, 0xe9, 0x02, 0xe0, 0x33, 0x21, 0x49, 0xd4,
	0xf4, 0x5c, 0xf1, 0xd1, 0x0b, 0x91, 0x30, 0xac, 0x7b, 0x6d, 0xb5, 0xc2, 0x44, 0x2f, 0x01, 0x65,
	0xbb, 0xaa, 0x6b, 0xfc, 0x06, 0x38, 0xd2, 0x92, 0x07, 0xd6, 0xb9, 0x5e, 0xff, 0xc4, 0x34, 0x3b
};

static int tm_gen_salt(int salt) {
	// Save for next block
	return 0;
}

static int tm_encrypt_block(u8 *data, u32 data_size, int salt) {
	u32 i;

	if (!data || !data_size)
		return -1;

	char next = 0;
	for (i = 0; i < data_size; ++i) {
		salt = tm_gen_salt(salt);
		
		next = (data[i] + salt + next);
		data[i] = next;
	}

	return 0;
}

static int tm_decrypt_block(u8 *data, u32 data_size, int salt) {
	u32 i;

	if (!data || !data_size)
		return -1;

	char next = 0, tmp = 0;
	for (i = 0; i < data_size; ++i) {
		salt = tm_gen_salt(salt);
		
		next = (data[i] - salt - tmp);
		tmp = data[i];
		data[i] = next;
	}

	return 0;
}

static int tm_encrypt_data(const char *key, int key_len, u8 *data, u32 data_size) {
	u32 i;

	if (!key || !key_len || !data || !data_size)
		return -1;

	char next = 0;
	for (i = 0; i < data_size; ++i) {
		next = (data[i] + key[i % key_len] + next);
		data[i] = next;
	}

	return 0;
}

static int tm_decrypt_data(const char *key, int key_len, u8 *data, u32 data_size) {
	u32 i;

	if (!key || !key_len || !data || !data_size)
		return -1;

	char next = 0, tmp = 0;
	for (i = 0; i < data_size; ++i) {
		next = (data[i] - key[i % key_len] - tmp);
		tmp = data[i];
		data[i] = next;
	}

	return 0;
}

static int sceSblSsEncryptWithPortability(u8 *data, u32 data_size, u8 key[16]) {
	u8 iv[16];
	mbedtls_aes_context aes;

	if (!data)
		return -1;

	memset(&aes, 0, sizeof(mbedtls_aes_context));
	memcpy(iv, key, 16);

	mbedtls_aes_setkey_enc(&aes, aes_key, 256);
	mbedtls_aes_crypt_cbc(&aes, MBEDTLS_AES_ENCRYPT, data_size, iv, data, data);

	return 0;
}

static int sceSblSsDecryptWithPortability(u8 *data, u32 data_size, u8 key[16]) {
	u8 iv[16];
	mbedtls_aes_context aes;

	if (!data)
		return -1;

	memset(&aes, 0, sizeof(mbedtls_aes_context));
	memcpy(iv, key, 16);

	mbedtls_aes_setkey_dec(&aes, aes_key, 256);
	mbedtls_aes_crypt_cbc(&aes, MBEDTLS_AES_DECRYPT, data_size, iv, data, data);

	return 0;
}

size_t snprintfcat(
	char* buf,
	size_t bufSize, const char* fmt,
	...) 
{
	size_t result;
	va_list args;
	size_t len = strnlen(buf, bufSize);

	va_start(args, fmt);
	result = vsnprintf(buf + len, bufSize - len, fmt, args);
	va_end(args);

	return result + len;
}

#ifdef __cplusplus
extern "C" {
#endif

	/*
	__declspec(dllexport) unsigned char *RetrieveKey(bool encrypt) {

		if (encrypt) {
			tm_encrypt_block(tmdb_json_hmac_key, TITLE_METADATA_HMAC_KEY_LEN, 0);
			tm_encrypt_data(tm_key, strlen(tm_key), tmdb_json_hmac_key, TITLE_METADATA_HMAC_KEY_LEN);

			sceSblSsEncryptWithPortability(tmdb_json_hmac_key, TITLE_METADATA_HMAC_KEY_LEN, aes_iv);
		} else {
			sceSblSsDecryptWithPortability(tmdb_json_hmac_key, TITLE_METADATA_HMAC_KEY_LEN, aes_iv);

			tm_decrypt_data(tm_key, strlen(tm_key), tmdb_json_hmac_key, TITLE_METADATA_HMAC_KEY_LEN);
			tm_decrypt_block(tmdb_json_hmac_key, TITLE_METADATA_HMAC_KEY_LEN, 0);
		}

		return tmdb_json_hmac_key;
	}
	*/

	__declspec(dllexport) const char* tmdb_gen_link(const char *np_title_id) {
#ifdef _DEBUG
		AllocConsole();
		AttachConsole(GetCurrentProcessId());
		
		errno_t err;
		FILE *fp;
		err = freopen_s(&fp, "CONOUT$", "w+", stdout);

		if (strcmp(np_title_id, "") == 0) {
			printf("[!] No np_title_id was inputted");
			return NULL;
		}
		printf("[*] Generating link for np_title_id: %s\n\n", np_title_id);
#endif
		sceSblSsDecryptWithPortability(tmdb_json_hmac_key, TITLE_METADATA_HMAC_KEY_LEN, aes_iv);

		tm_decrypt_data(tm_key, strlen(tm_key), tmdb_json_hmac_key, TITLE_METADATA_HMAC_KEY_LEN);
		tm_decrypt_block(tmdb_json_hmac_key, TITLE_METADATA_HMAC_KEY_LEN, 0);
		
		u8 digest[20];
		const char *json_link;

		json_link = (const char*)malloc(TITLE_METADATA_LINK_MAX_LEN);

		memset(digest, 0, TITLE_METADATA_HMAC_DIGEST_LEN);
		memset((void*)json_link, 0, TITLE_METADATA_LINK_MAX_LEN);

		snprintfcat((char*)json_link, TITLE_METADATA_LINK_MAX_LEN, "http://tmdb.np.dl.playstation.net/tmdb2/%s_", np_title_id);

		// Use a standard implementation of SHA-1 HMAC.
		mbedtls_sha1_hmac(tmdb_json_hmac_key, TITLE_METADATA_HMAC_KEY_LEN, (u8*)np_title_id, strlen(np_title_id), digest);
		for (int i = 0; i < TITLE_METADATA_HMAC_DIGEST_LEN; ++i)
			snprintfcat((char*)json_link, TITLE_METADATA_LINK_MAX_LEN, "%02X", digest[i]);

		snprintfcat((char*)json_link, TITLE_METADATA_LINK_MAX_LEN, "/%s.json", np_title_id);
		tm_encrypt_block(tmdb_json_hmac_key, TITLE_METADATA_HMAC_KEY_LEN, 0);
		tm_encrypt_data(tm_key, strlen(tm_key), tmdb_json_hmac_key, TITLE_METADATA_HMAC_KEY_LEN);

		sceSblSsEncryptWithPortability(tmdb_json_hmac_key, TITLE_METADATA_HMAC_KEY_LEN, aes_iv);

#ifdef _DEBUG
		printf("\n\n[*] The link has been generated successfully\n\n");
		FreeConsole();
#endif
		
		return json_link;
	}
}
