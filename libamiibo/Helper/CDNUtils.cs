/*
 * Copyright (C) 2016 Benjamin Krämer
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.IO;
using System.Linq;
using System.Net;
using LibAmiibo.Data.Settings;
using LibAmiibo.Data.Settings.TitleID;

namespace LibAmiibo.Helper
{
    public static class CDNUtils
	{
        static CDNUtils()
        {
            // Accept nintendos certificate:
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => Equals("B48ECF5C04B9CBB18D6215D0AD51DAA113929DE8", certificate.GetCertHashString());
        }

		public static IDBEContext DownloadTitleData(Title title)
        {
            var cdnKeys = Files.CDNKeys;
		    if (cdnKeys == null)
		        return null;

            string titleId = title.TitleID.ToString("X16").ToUpper();

            string metadataUrl;
            if (title.Platform == Platform.N3DS)
                metadataUrl = $"https://idbe-ctr.cdn.nintendo.net/icondata/{10}/{titleId}.idbe";
            else if (title.Platform == Platform.WiiU)
                metadataUrl = $"https://idbe-wup.cdn.nintendo.net/icondata/{10}/{titleId}.idbe";
            else return null;

			byte[] data;
            try
            {
                using (var webClient = new WebClient())
                {
                    data = webClient.DownloadData(metadataUrl);
                }
            }
            catch (WebException ex)
            {
                return null;
            }

            var dataSkip2 = data.Skip(2).ToArray();
			var keyslot = data[1];
		    var iconData = cdnKeys.DecryptIcon(dataSkip2, keyslot);

            if (title.Platform == Platform.WiiU)
		    {
		        using (var memoryStream = new MemoryStream(iconData))
		        {
		            IDBEWiiUContext context = new IDBEWiiUContext();
		            context.Open(memoryStream);
		            return context;
		        }
		    }
		    if (title.Platform == Platform.N3DS)
			{
                // retrieve image (might get used later?)
                using (var memoryStream = new MemoryStream(iconData))
                {
                    IDBE3DSContext context = new IDBE3DSContext();
                    context.Open(memoryStream);
                    return context;
                }
			}

		    return null;
        }
	}
}