using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DictionaryParser
{
    public class Program
    {
        private static bool Active = true;
        static void Main()
        {
            while (Active)
            {
                Console.WriteLine("Выберите действие:\n1 - Добавить\n2 - Заменить");
                switch (Console.ReadLine())
                {
                    case "1":
                        Add();
                        break;
                    case "2":
                        Replace();
                        break;
                    default:
                        continue;
                }
            }
        }

        private static void Add()
        {
            Dictionary dictionary = new Dictionary();
            Console.Write("Введите ключ:");
            dictionary.Key = Console.ReadLine();
            
            Console.Write("Введите значение:");
            dictionary.Value = Console.ReadLine();
            List<Dictionary> dictionaries = SaveLoad.Load();
            if (dictionaries.Where(x => x.Key == dictionary.Key).FirstOrDefault() != default)
            {
                Console.WriteLine("Значение с таким ключем уже существует.");
                return;
            }
            dictionaries.Add(dictionary);
            SaveLoad.Save(dictionaries);
            Console.WriteLine("Текущие данные:");
            foreach (Dictionary dict in dictionaries)
            {
                Console.WriteLine("Ключ - '{0}'; значение - '{1}'", dict.Key, dict.Value);
            }
        }

        private static void Replace()
        {
            Console.Write("Введите ключ:");
            string key = Console.ReadLine();
            List<Dictionary> dictionaries = SaveLoad.Load();
            Console.WriteLine("Выполняется поиск по ключу...");
            if (dictionaries.Where(x => x.Key == key).FirstOrDefault() == default)
            {
                Console.WriteLine("Ключ не найден.");
            }
            else
            {
                Console.WriteLine("Значение по ключу - '{0}'", dictionaries.Where(x => x.Key == key).FirstOrDefault().Value);
                Console.WriteLine("Введите новое значение:");
                string value = Console.ReadLine();
                dictionaries.Where(x => x.Key == key).FirstOrDefault().Value = value;
                SaveLoad.Save(dictionaries);
                Console.WriteLine("Текущие данные:");
                foreach (Dictionary dict in dictionaries)
                {
                    Console.WriteLine("Ключ - '{0}'; значение - '{1}'", dict.Key, dict.Value);
                }
            }
        }

    }


    public static class SaveLoad
    {
        private static readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof(List<Dictionary>));
        private const string FileName = "File.xml";

        public static bool Save(List<Dictionary> dictionaries)
        {
            FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate);
            try
            {
                XmlSerializer.Serialize(fs, dictionaries);
                fs.Close();
                return true;
            }
            catch (InvalidOperationException)
            {
                fs.Close();
                return false;
            }
        }

        public static List<Dictionary> Load()
        {
            FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate);
            List<Dictionary> dictionaries = new List<Dictionary>();
            try
            {
                dictionaries = (List<Dictionary>)XmlSerializer.Deserialize(fs);
                fs.Close();
                return dictionaries;
            }
            catch (InvalidOperationException)
            {
                fs.Close();
                return dictionaries;
            }
        }
    }


    [Serializable]
    public class Dictionary
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

}
