using FluentValidation;
using MongoDB.Bson;
using System.Globalization;

namespace CloudConsult.Common.Validators
{
    public class ApiValidator<T> : AbstractValidator<T>
    {
        public bool BeValidMongoDbId(string value)
        {
            return ObjectId.TryParse(value, out var bsonId);
        }

        public bool BeValidGuid(string value)
        {
            return Guid.TryParse(value, out var guid);
        }

        public bool HaveSomeValues(Dictionary<string, List<string>> map)
        {
            return map != null && map.Count != 0;
        }

        public bool HaveValidDateFormat(string date)
        {
            return DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var outDate);
        }

        public bool BeAlphaNumeric(string value)
        {
            return value.All(x => char.IsLetterOrDigit(x));
        }

        public bool BeAlphabetsOnly(string value)
        {
            return value.All(x => char.IsLetter(x));
        }

        public bool BeNumericOnly(string value)
        {
            return value.All(x => char.IsDigit(x));
        }

        public bool BeAlphabetsOrWhitespaceOnly(string value)
        {
            return value.All(x => char.IsLetter(x) || char.IsWhiteSpace(x));
        }
    }
}
