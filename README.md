libamiibo
========
amiibo™ parsing library

Usage
=====
**libamiibo expects a binary dump. It will not work with XMLs or hexadecimal text files**. 

It offers support for encryption/decryption, figurine data, amiibo settings and AppData.

To parse a tag directly from a binary (encrypted) dump, use ```LibAmiibo.Data.AmiiboTag.FromNtagData()```.

When using an encrypted binary, the AmiiboSettings and AppData information will not contain any valid information.

Examples
--------

- Decrypting a NTAG215 dump "mario.bin" with the "retail.bin" keys:
```
byte[] encryptedNtagData = System.IO.File.ReadAllBytes("mario.bin");
var keys = LibAmiibo.Encryption.AmiiboKeys.LoadKeys("retail.bin");
LibAmiibo.Data.AmiiboTag amiiboTag = LibAmiibo.Data.AmiiboTag.DecryptWithKeys(keys, encryptedNtagData);
```

- Parsing a NTAG215 dump "mario.bin" only for the non-encrypted information:
```
byte[] encryptedNtagData = System.IO.File.ReadAllBytes("mario.bin");
var keys = LibAmiibo.Encryption.AmiiboKeys.LoadKeys("retail.bin");
LibAmiibo.Data.AmiiboTag amiiboTag = LibAmiibo.Data.AmiiboTag.FromNtagData(encryptedNtagData);
```

Special Thanks
==============
- Lucas "MacGuffen" Romo for his great work with https://docs.google.com/spreadsheets/d/1WJ4HxS9hkLquq-ATt1Rq9mioH6RDgP3qQrtYVaOdimM
- The people over at 3dbrew.org for there work on http://3dbrew.org/wiki/Amiibo
- socram8888 for his great work on https://github.com/socram8888/amiitool
- John "LouieGeetoo" Pray for his great work with http://www.amiibo.life