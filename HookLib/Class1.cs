using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HookLib
{
    public class Data
    {
        public string Title { get; set; }
        public string Sub { get; set; }

        public Data(string title, string sub)
        {
            Title = title;
            Sub = sub;
        }
    }

    public static class Address
    {
        public static Data[] AdressParts(this string entryAddr)
        {
            int counter = 0;
            int i = 0;
            int priority = 0;

            string strComp = "";
            string strDef = "";
            string[] parts = entryAddr.Split(',');
            if (parts.Length <= 1)
                throw new ArgumentException("Невозможно разобрать строку", "entryAddr");

            Data[] data = new Data[7];

            for (i = 0; i < parts.Length; i++)
            {
                parts[i] = parts[i].ToLower(new System.Globalization.CultureInfo("ru-RU", false));
                while (parts[i][0] == ' ')
                    parts[i] = parts[i].Remove(0, 1);
                switch (parts[i][0])
                {
                    case 'г':
                        strComp = "город";
                        strDef = "г. ";
                        priority = 2;
                        break;
                    case 'д':
                        if (parts[i].Any(char.IsDigit))
                        {
                            strComp = "дом";
                            priority = 4;
                        }
                        else
                        {
                            strComp = "деревня";
                            priority = 3;
                        }
                        strDef = "д. ";
                        break;
                    case 'к':
                        if (parts[i][1] == 'в')
                        {
                            strComp = "квартира";
                            strDef = "кв. ";
                            priority = 6;
                        }
                        else
                        {
                            strComp = "корпус";
                            strDef = "корп. ";
                            priority = 5;
                        }
                        break;
                    case 'п':
                        strComp = "поселок";
                        strDef = "пос. ";
                        priority = 3;
                        break;
                    case 'р':
                        strComp = "район";
                        strDef = "р-н ";
                        priority = 2;
                        break;
                    case 'с':
                        strComp = "село";
                        strDef = "с. ";
                        priority = 3;
                        break;
                    case 'у':
                        strComp = "улица";
                        strDef = "ул. ";
                        priority = 3;
                        break;
                }
                if (parts[i].Contains("край") || parts[i].Contains("область"))
                {
                    strComp = "";
                    strDef = " ";
                    priority = 1;
                }
                if (parts[i].Length >= 6 && parts[i].All(char.IsDigit))
                {
                    strComp = "";
                    strDef = "";
                    priority = 0;
                }
                for (int j = 0; j < strComp.Length; j++)
                {
                    while (parts[i][0].Equals(strComp[j]) && parts[i].Length > 0)
                    {
                        parts[i] = parts[i].Remove(0, 1);
                        counter++;
                    }
                }
                while (parts[i].Length > 0 && (parts[i][0].Equals(' ') || parts[i][0].Equals('.')))
                {
                    parts[i] = parts[i].Remove(0, 1);
                }
                counter = 0;
                while (parts[i][parts[i].Length - 1] == ' ')
                    parts[i] = parts[i].Remove(parts[i].Length - 2, 1);
                parts[i] = parts[i][0].ToString().ToUpper(new System.Globalization.CultureInfo("ru-RU", false)) + parts[i].Substring(1);
                if (data[priority] == null)
                    data[priority] = new Data(parts[i], strDef);
            }
            if (data[0] == null)
                data[0] = new Data("614000", "");
            if (data[1] == null)
                data[1] = new Data("Пермский край", " ");
            if (data[2] == null)
                data[2] = new Data("Пермь", "г. ");
            return data;
        }

        public static string CorrectAddress(this Data[] data)
        {
            string correctAddr = "";
            int i = 0;
            for (i = 0; i < data.Length; i++)
            {
                if (data[i] != null)
                {
                    correctAddr += data[i].Sub + data[i].Title;
                    if (i != data.Length - 1)
                        correctAddr += ", ";
                }
            }
            return correctAddr;
        }
    }
}
