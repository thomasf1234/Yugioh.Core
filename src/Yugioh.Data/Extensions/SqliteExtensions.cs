using Microsoft.Data.Sqlite;
using System;

namespace Yugioh.Data.Extensions
{
    public static class SqliteExtensions
    {
        public static SqliteParameter AddWithNullableValue(
    this SqliteParameterCollection collection,
    string parameterName,
    object value)
        {
            if (value == null)
                return collection.AddWithValue(parameterName, DBNull.Value);
            else
                return collection.AddWithValue(parameterName, value);
        }
    }
}
