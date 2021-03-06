﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace YaVoice
{
    public enum format { mp3, wav, opus };
    public enum quality { hi, lo };
    public enum speaker { jane, oksana, alyss, omazh, zahar, ermil };
    public enum emotion { good, neutral, evil }

    class Speech
    {
        static String URL = @"https://tts.voicetech.yandex.net/generate";



      

        /// <summary>
        /// Сгенерировать синтезированую речь из текста
        /// </summary>
        /// <param name="key">ключ доступа</param>
        /// <param name="text">текст для синтеза</param>
        /// <param name="_format">формат получаемого файла</param>
        /// <param name="lang">язык</param>
        /// <param name="_speaker">голос синтеза</param>
        /// <param name="_quality">качество получаемого файла</param>
        /// <param name="speed">скорость речи</param>
        /// <param name="_emotion">эмоция речи</param>
        public static Stream Synthes(string key, string text, format _format,
            speaker _speaker, quality _quality = quality.lo, double speed = 1.0,
            emotion _emotion = emotion.good)
        {
            string getUrl = String.Format(URL + "?text={0}&format={1}&lang=ru-RU&speaker={2}&emotion={3}&quality={4}&speed={5}&key={6}",
                text, _format, _speaker, _emotion, _quality, speed.ToString().Replace(',', '.'), key);
            try
            {
                WebRequest req = WebRequest.Create(getUrl);
                WebResponse resp = req.GetResponse();
                Stream stream = resp.GetResponseStream();
                // StreamReader sr = new System.IO.StreamReader(stream);
                //StreamWriter wr = new StreamWriter(stream);

                //   SoundPlayer player = new SoundPlayer(stream);
                //   player.PlaySync();
                //   return true;
                return stream;
            }
            catch (Exception) { return null; }

        }



        //https://tts.voicetech.yandex.net/generate?text=Проверка синтеза речи
        //&format=mp3&lang=ru-RU&speaker=jane&emotion=good
        //&key=83ca8eb1-9758-429d-a575-210eb27a3ad4


    }
}
