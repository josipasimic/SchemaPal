﻿namespace SchemaPal.Enums.EnumTranslators
{
    public static class KeyTypeTranslator
    {
        public static string MapToName(KeyType columnKeyType)
        {
            switch (columnKeyType)
            {
                case KeyType.None:
                    return "Nije odabrano";
                case KeyType.Primary:
                    return "Primarni ključ";
                case KeyType.Foreign:
                    return "Strani ključ";
                case KeyType.Unique:
                    return "Jedinstveni ključ";
                default:
                    return string.Empty;
            }
        }

        public static string GetAbbreviation(KeyType columnKeyType)
        {
            switch (columnKeyType)
            {
                case KeyType.None:
                    return string.Empty;
                case KeyType.Primary:
                    return "PK";
                case KeyType.Foreign:
                    return "FK";
                case KeyType.Unique:
                    return "UQ";
                default:
                    return string.Empty;
            }
        }
    }
}
