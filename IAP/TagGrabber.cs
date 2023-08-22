using InstagramApiSharp;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;

namespace IAP
{
    public class TagGrabber
    {
        private readonly UserSessionData _user;
        private readonly string _outputDirectory;

        public TagGrabber(UserSessionData user, string outputDirectory)
        {
            _user = user;
            _outputDirectory = outputDirectory;
        }

        public async Task GrabByTag()
        {
            Console.WriteLine("Введи tag");
            var hashtag = Console.ReadLine();
            var downloadPath = _outputDirectory; 

            var api = InstaApiBuilder.CreateBuilder()
                .SetUser(_user)
                .Build();

            if (!api.IsUserAuthenticated)
            {
                var loginResult = await api.LoginAsync();
                if (!loginResult.Succeeded)
                {
                    Console.WriteLine($"Failed to login: {loginResult.Info.Message}");
                    return;
                }
            }

            await Console.Out.WriteLineAsync("User logged");

            var lastId = string.Empty;
            var tagFeed = await api.HashtagProcessor.GetTopHashtagMediaListAsync(hashtag, PaginationParameters.MaxPagesToLoad(500));

            for (int i = 0; i < 500; i++)
            {

                if (!string.IsNullOrEmpty(lastId))
                {
                    tagFeed = await api.HashtagProcessor.GetTopHashtagMediaListAsync(hashtag, PaginationParameters.MaxPagesToLoad(500).StartFromMaxId(lastId));
                }



                if (!tagFeed.Succeeded)
                {
                    Console.WriteLine($"Failed to retrieve hashtag media: {tagFeed.Info.Message}");
                    return;
                }

                await Console.Out.WriteLineAsync("Feed gated");

                foreach (var mediaItem in tagFeed.Value.Medias)
                {
                    var owner = mediaItem.User.UserName;

                    foreach (var image in mediaItem.Images)
                    {
                        var imageUrl = image.Uri;
                        var imageBytes = await api.HttpClient.GetByteArrayAsync(imageUrl);

                        var fileNameWithoutExtension = $"{owner}";
                        var fileExtension = ".jpg";
                        var fileName = $"{fileNameWithoutExtension}{fileExtension}";
                        var filePath = Path.Combine(downloadPath, fileName);

                        int suffix = 1;
                        while (File.Exists(filePath))
                        {
                            fileNameWithoutExtension = $"{owner}_{suffix}";
                            fileName = $"{fileNameWithoutExtension}{fileExtension}";
                            filePath = Path.Combine(downloadPath, fileName);
                            suffix++;
                        }

                        File.WriteAllBytes(filePath, imageBytes);

                        Console.WriteLine($"Image saved: {filePath}");
                    }
                }

                lastId = tagFeed.Value.Medias.LastOrDefault()?.InstaIdentifier;
            }



            Console.WriteLine("Image downloading completed.");
        }

    }

}
