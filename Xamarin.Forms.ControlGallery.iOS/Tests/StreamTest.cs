using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using NUnit.Framework;
using UIKit;

namespace Xamarin.Forms.ControlGallery.iOS.Tests
{
	[TestFixture]
	public class StreamTest
	{
		[Test(ExpectedResult = true)]
		public async Task<bool> GetStreamAsyncThrowsOnFakeURI2()
		{
			var uri = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/96/Portrait_Of_A_Baboon.jpg/314px-Portrait_Of_A_Baboon.jpg";

			var result = await Task.Run(async () =>
			{
				try
				{
					var images = Enumerable.Range(0, 10)
						.Select(x => new UriImageSource() { CachingEnabled = false, Uri = new Uri(uri + "?id=" + x.ToString()) })
						.Select(x => x.GetStreamAsync())
						.ToArray();

					var streams = await Task.WhenAll(images).ConfigureAwait(false);

					foreach(var stream in streams)
					{
						if (stream == null)
							return false;

						var image = UIImage.LoadFromData(NSData.FromStream(stream));

						if (image == null || image.Size == CGSize.Empty)
							return false;
					}

					return true;

				}
				catch(Exception exc)
				{
					Assert.Fail(exc.ToString());
				}

				return false;

			}).ConfigureAwait(false);

			return result;
		}
	}
}
