using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;

namespace YaVoice
{
    class Program
    {
        static void Main(string[] args)
        {
            /* string s = "-k dfsdfhsdkfjhsdkf-sdfsdf-sd-f-sdf -s 1.0 -v zahar";
             string[] parts = Regex.Split(s, " (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
             foreach (string part in parts)
                 Console.WriteLine(part);
             Console.ReadLine();*/

            if (args.Count() > 0)
            {
                if (args[0] == "-h")
                {
                    Console.WriteLine("Синтезатор речи с использованием сервисов Яндекс\n\n");
                    Console.WriteLine("Использование:\n");
                    Console.WriteLine("\t-k <ключ> -t <текст> [-f <mp3,wav>] [-v <jane, oksana, alyss, omazh, zahar, ermil>]");
                    Console.WriteLine("\t[-q <h,l>] [-s <скорость>] [-e <g, n, e>] [-o <файл>] [-s] \n\n");
                    Console.WriteLine("Параметры:\n");
                    Console.WriteLine("\t-k\t\tAPI-ключ");
                    Console.WriteLine("\t-t\t\tТекст, который нужно озвучить. \n\t\t\tДля передачи слов-омографов используйте + перед ударной гласной. Например, гот+ов или def+ect.");
                    Console.WriteLine("\t-f\t\tРасширение синтезируемого файла (его формат). Допустимые значения:\n\t\t\t\tmp3 — аудио в формате MPEG\n\t\t\t\twav — аудио в формате PCM 16 бит");
                    Console.WriteLine("\t-v\t\tГолос синтезированной речи. Можно выбрать один из следующих голосов:\n\t\t\t\tjane, oksana, alyss, omazh, zahar, ermil.");
                    Console.WriteLine("\t-q\t\tЧастота дискретизации и битрейт синтезируемого PCM-аудио (медиаконтейнер WAV). Допустимые значения:\n\t\t\t\th — частота дискретизации 48 кГц и битрейт 768 кб/c;\n\t\t\t\tl — частота дискретизации 8 кГц и битрейт 128 кб/c.");
                    Console.WriteLine("\t-s\t\tСкорость (темп) синтезированной речи. \n\t\t\tСкорость речи задается дробным числом в диапазоне от 0.1 до 3.0");
                    Console.WriteLine("\t-e\t\tЭмоциональная окраска голоса. Допустимые значения: \n\t\t\t\tg — радостный, доброжелательный;\n\t\t\t\te — раздраженный;\n\t\t\t\tn — нейтральный (используется по умолчанию).");
                    Console.WriteLine("\t-p\t\tВоспроизвести озвученный текст. (Только в wav-формате)");
                    Console.WriteLine("\t-o\t\tСохранить в файл");
                }


                string key = String.Empty, text = String.Empty;
                format format = format.wav;
                speaker voice = speaker.jane;
                double speed = 1.0;
                quality quality = quality.lo;
                emotion emotion = emotion.neutral;

                bool isPlay = false, isSave = false;
                string file = "";
                try
                {
                    for (int i = 0; i < args.Count(); i++)
                    {

                        switch (args[i])
                        {
                            case "-k":
                                key = args[i + 1];
                                //Console.WriteLine("key is " + args[i + 1]);
                                break;
                            case "-t":
                                text = args[i + 1];
                                break;

                            case "-f":
                                switch (args[i + 1])
                                {
                                    case "wav":
                                        format = format.wav;
                                        break;
                                    case "mp3":
                                        format = format.mp3;
                                        break;
                                    default:
                                        format = format.wav;
                                        break;
                                }
                                break;
                            case "-v":
                                switch (args[i + 1])
                                {
                                    case "alyss":
                                        voice = speaker.alyss;
                                        break;
                                    case "ermil":
                                        voice = speaker.ermil;
                                        break;
                                    case "jane":
                                        voice = speaker.jane;
                                        break;
                                    case "oksana":
                                        voice = speaker.oksana;
                                        break;
                                    case "omazh":
                                        voice = speaker.omazh;
                                        break;
                                    case "zahar":
                                        voice = speaker.zahar;
                                        break;
                                    default:
                                        voice = speaker.jane;
                                        break;
                                }
                                break;

                            case "-q":
                                switch (args[i + 1])
                                {
                                    case "h":
                                        quality = quality.hi;
                                        break;
                                    case "l":
                                        quality = quality.lo;
                                        break;
                                    default:
                                        quality = quality.lo;
                                        break;
                                }
                                break;
                            case "-s":
                                speed = Double.TryParse(args[i + 1].Replace(',', '.'), out speed) ? Double.Parse(args[i + 1].Replace(',', '.')) : 1.0;
                                break;

                            case "-e":
                                switch (args[i + 1])
                                {
                                    case "g":
                                        emotion = emotion.good;
                                        break;
                                    case "n":
                                        emotion = emotion.neutral;
                                        break;
                                    case "e":
                                        emotion = emotion.evil;
                                        break;
                                    default:
                                        emotion = emotion.neutral;
                                        break;
                                }

                                break;

                            case "-p":
                                isPlay = true;
                                break;
                            case "-o":
                                isSave = true;
                                file = args[i + 1];
                                break;
                        }
                    }

                    if (String.IsNullOrEmpty(key) && String.IsNullOrEmpty(text))
                    { Console.WriteLine("Невеный формат команд"); return; }

                }
                catch (Exception) { Console.WriteLine("Невеный формат команд"); }



                MemoryStream stream = new MemoryStream(); Speech.Synthes(key, text, format, voice, quality, speed, emotion).CopyTo(stream);
                if (stream == null) { Console.WriteLine("Не удалось синтезировать"); return; }


                if (isPlay)
                {
                    SoundPlayer player = new SoundPlayer(stream);
                    player.PlaySync();
                }
                int f = 0;
                try
                {
                    if (isSave)
                    {
                        f = 1;

                        // запись в файл
                        using (var sr1 = new FileStream(file, FileMode.Create))
                        {
                            f = 2;

                            
                            // преобразуем строку в байты
                            byte[] bytesInStream = stream.ToArray();
                            stream.Read(bytesInStream, 0, bytesInStream.Length);
                            f = 3;
                            // Use write method to write to the file specified above
                            sr1.Write(bytesInStream, 0, bytesInStream.Length);
                        }
                        /*
                        using (FileStream fileStream = File.Create(file, (int)stream.Length))
                        {
                            f = 2;
                            byte[] data = new byte[stream.Length];

                            // stream.Read(data, 0, (int)data.Length);
                            fileStream.Write(data, 0, data.Length);
                        }*/
                    }
                }
                catch(Exception ex) { Console.WriteLine(ex.Message+" "+f); }


                // Speech.Synthes("","", format.mp3,,quality.lo,,)
            }
        }
    }
}
