﻿using System;
using InstagramApiSharp;

using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using System.Threading.Channels;
using IAP;

namespace InstagramHashtagImageDownloader
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var directory = "C:\\Users\\fedunov\\Desktop\\c";

            var menu = """
                Выбери опцию:
                1 - поиск фото по тегу
                2 - собрать имена файлов в список
                """;
            await Console.Out.WriteLineAsync(menu);

            var option = Console.ReadLine();

            switch (option)
            {
                case "1":

                    var userSessionData = new UserSessionData();

                    await Console.Out.WriteLineAsync("Введи логин:");
                    userSessionData.UserName = Console.ReadLine();

                    await Console.Out.WriteLineAsync("Введи пароль");
                    userSessionData.Password = Console.ReadLine();

                    var tagGrabber = new TagGrabber(userSessionData, directory);
                    await tagGrabber.GrabByTag();
                    break;
                case "2":
                    var photoNamesList = new PhotoNamesListFormer(directory);
                    photoNamesList.NamesFormer();
                    break;

                default:
                    break;
            }         


        }
    }
}
