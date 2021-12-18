using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace CarStore591
{
    class Bot
    {
        private TelegramBotClient bot;
        private BonusSystem bs = new BonusSystem();
        private string[] msg = new string[5];
        private Dictionary<string, (string command, int counter, int steps)> commandList = 
           new Dictionary<string, (string command, int counter, int steps)>
       {
            { "login" , ("login", 1, 2) },
             { "registration" , ("registration", 1, 6) }
        };
        private Dictionary<long, List<string>> msg1 = new Dictionary<long, List<string>>(); // contains messages from different chats
        private Dictionary<long, int?> userCode = new Dictionary<long, int?>(); // contains codes of authorized users
        private Dictionary<long, Command> cmd = new Dictionary<long, Command>(); // contains current user commands 

        public Bot()
        {
            bot = new TelegramBotClient(Config.TOKEN);
            bot.OnMessage += Bot_OnMessage;
            bot.StartReceiving();
            Console.WriteLine("Все ок, бот работает");
            Console.ReadLine();
            bot.StopReceiving();
        }


        private static string About()
        {
            return "Для авторизации в сервисе введите команду /login." +
                "\nесли вы еще не зарегистрированы в нашей системе, то просим вас перейти в бот https://t.me/Bonus_point_system_bot и выполнить его дальнейшие указания." +
                "\nЧтобы увидеть список команд, введите /help";

        }
        private static string Help()
        {
            return "Список команд бота:" +
                "\n/login - авторизоваться" +
                 "\n/cars - вывод каталога машин" +
                 "\n/cars+номер - вывод определённой машины из каталога" +
                 "\n/buy+номер - купить машину" +
                 "\n/friends - друзья бота";

        }
        private static string Neighbors()
        {
            return "Наши друзья: " +
                "\nhttps://t.me/Bonus_point_system_bot - зарегистрирует вас в нашей системе" +
                "\nhttps://t.me/car_service_Kristina_bot - предоставит вам услуги автосервиса";

        }
        private static string Error()
        {
            return ("Машины с введённым номером не существует");
        }

        private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            long id = e.Message.Chat.Id;
            var text = e.Message.Text;
            var message = e.Message;
            bool isValid;
            if (message.Type != MessageType.Text || message == null)
                //Проверка на отсутствие текста в сообщении, в ходе которой изображения и пустые сообщения (в случае багов самого телеграма) игнорируются
                return;
            msg = message.Text.Split(' ');
           
            if (!msg1.ContainsKey(id))
            {
                msg1.Add(id, new List<string>());
                userCode.Add(id, null);
                cmd.Add(id, null);
            }
            msg1[id].AddRange(msg);
            Console.WriteLine($"{ e.Message.Chat.FirstName} { String.Join(" | ", msg1[id])}");
            

            switch  (msg[0]) //Блок команд
            {
                case "/start":   //Действия при команде "старт"
                                 //Текст приветственного сообщения
                    string textS = "Команда " + message.Text + '\n' + About();
                    await bot.SendTextMessageAsync(message.Chat.Id, textS);
                    break;

                case "/help":
                    string textH = "Команда " + message.Text + '\n' + Help();
                    await bot.SendTextMessageAsync(message.Chat.Id, textH);
                    break;
                case "/friends":
                    string textF = "Команда " + message.Text + '\n' + Neighbors();
                    await bot.SendTextMessageAsync(message.Chat.Id, textF);
                    break;

                case "/cars":
                    if (msg.Count() > 1)
                    if (int.TryParse(msg[1], out int number))
                    {
                        try
                        {
                            await bot.SendTextMessageAsync(message.Chat.Id, Config.Cars[number - 1]);
                        }
                        catch
                        {
                            string textE = "Некорректный номер машины";
                            await bot.SendTextMessageAsync(message.Chat.Id, textE);
                        }
                        break;
                    }
                    await bot.SendTextMessageAsync(message.Chat.Id,
                     String.Join("\n", Config.Cars).ToString());
                    break;
                case "/login":
                    {
                        msg1[id].Clear();
                        cmd[id] = new Command(commandList["login"]);
                        bot.SendTextMessageAsync(id, "Введите логин:");
                    }
                    break;
                case "/buy":
                    {
                        bot.SendTextMessageAsync(id, "Вы собираетесь приобрести:");
                        if (msg.Count() > 1)
                            if (int.TryParse(msg[1], out int number))
                            {
                            try
                            {   
                                await bot.SendTextMessageAsync(id, Config.Cars[number - 1]);
                                bot.SendTextMessageAsync(id, "/Yes | /No");
                                
                                }
                                catch
                            {
                                string textE = "Некорректный номер машины";
                                await bot.SendTextMessageAsync(message.Chat.Id, textE);
                            }
                        }
                    }
                    break;
                case "/Yes":
                    {
                        bot.SendTextMessageAsync(id, "Скоро с вами свяжется администратор:)");
                    }
                    break;
                case "/No":
                    {
                        bot.SendTextMessageAsync(id, "Ну что ж, не в этот раз...");
                    }
                    break;
                default:
                    {
                        switch (cmd[id]?.command ?? string.Empty)
                        {
                            case "login":
                                if (cmd[id].counter == cmd[id].steps)
                                {
                                    try
                                    {
                                        userCode[id] = bs.Account.Authorization(msg1[id][0], msg1[id][1]);
                                         bot.SendTextMessageAsync(id, "Вы успешно вошли в систему!");
                                        break;
                                    }
                                    catch (Exception)
                                    {
                                         bot.SendTextMessageAsync(id, "Неправильный логин или пароль");
                                        break;
                                    }
                                }
                                else
                                {
                                    if (cmd[id].counter == 1)
                                    {
                                        cmd[id].counter++;
                                        bot.SendTextMessageAsync(id, "Введите пароль:");
                                        break;
                                    }
                                }
                                break;
                            default:
                                bot.SendTextMessageAsync(id, "Неизвестная команда");
                                break;
                        }
                        break;
                    }
            }
        }
   }
}

   
