using System;
using System.Collections.Generic;
using Telegram.Bot;
using System.IO;

namespace Exemple_9
{
    class Program
    {
        static string token = "1982991683:AAGHIoMbqau49ljCVd3Kb_w8NSa6qXowaBM";
        static TelegramBotClient client;
        static bool starting = false;
        static int number = 0;
        static void Main(string[] args)
        {
            client = new TelegramBotClient(token);
            client.StartReceiving();

            client.OnMessage += Client_OnMessage;
            Console.ReadKey();


        }
        static List<string> FileId = new List<string>();

        /// <summary>
        /// Меню бота
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Client_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (starting == false)
            {
                client.SendTextMessageAsync(new Telegram.Bot.Types.ChatId(e.Message.Chat.Id), "Введите /start для запуска бота. ");
                if (e.Message.Text == "/start")
                {
                    starting = true;
                }
            }
            else
            {
                client.SendTextMessageAsync(new Telegram.Bot.Types.ChatId(e.Message.Chat.Id), "Перенесите файл для загрузки его в бота, " +
                          "\nвведите /list для получения списка загруженных файлов.");
                // "\nВведите /download и ключ обьекта для загрузки файла на ПК.");
                if (e.Message.Text == "/list")
                {
                    if (number > 0)
                    {
                        string text = number.ToString() + " " + FileId[number - 1];
                        client.SendTextMessageAsync(new Telegram.Bot.Types.ChatId(e.Message.Chat.Id), text);
                    }
                    else
                    {
                        client.SendTextMessageAsync(new Telegram.Bot.Types.ChatId(e.Message.Chat.Id), "Нет загруженных файлов!");
                    }
                }
                    if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
                    {
                        FileId.Add(e.Message.Document.FileName);


                        Console.WriteLine(number+" "+e.Message.Document.FileName);
                       

                        DownLoad(e.Message.Document.FileId, e.Message.Document.FileName);
                    }

                    Console.WriteLine(e.Message.Text);
                }

            }





            /// <summary>
            /// скачивание файла
            /// </summary>
            /// <param name="fileId"></param>
            /// <param name="path"></param>
            static async void DownLoad(string fileId, string path)
            {
                var file = await client.GetFileAsync(fileId);
                FileStream fs = new FileStream("_" + path, FileMode.Create);
                await client.DownloadFileAsync(file.FilePath, fs);
                fs.Close();

                fs.Dispose();

            }

        }
    }

