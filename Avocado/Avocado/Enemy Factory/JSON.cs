using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace Avocado
{
    /*Dyl's big Class. Reads a Json string (One line from our level file) and returns the level object 
     This allows us to have optional parameters for each wave, as well as allowing each wave to have multiple enemies.
     
     Sample JSON string:
        {"waves":[{"waveid":1,"timer":5000,"monsterid":1,"monster_num":5},{"waveid":2,"timer":2500,"monsterid":1,"monster_num":10}], "additional_info":"Speed, background graphic, available powerups, etc."}
     
        This is a bit of a rough first draft, but essentially it contains the data for 2 waves of enemies to spawn. wave 1 Spawns 5 basic monsters 5 seconds into the level.
            Wave 2 spawns 10 of the same enemy 2.5 seconds after the first wave appeared.
        Additional info can hold whatever other values we want to give to the level.
     */
    public class JSON
	{
		public const int TOKEN_NONE = 0;
		public const int TOKEN_CURLY_OPEN = 1;
		public const int TOKEN_CURLY_CLOSE = 2;
		public const int TOKEN_SQUARED_OPEN = 3;
		public const int TOKEN_SQUARED_CLOSE = 4;
		public const int TOKEN_COLON = 5;
		public const int TOKEN_COMMA = 6;
		public const int TOKEN_STRING = 7;
		public const int TOKEN_NUMBER = 8;
		public const int TOKEN_TRUE = 9;
		public const int TOKEN_FALSE = 10;
		public const int TOKEN_NULL = 11;

		private const int BUILDER_CAPACITY = 2000;

		/// <summary>
		/// Parses the string json into a value
		/// </summary>
		/// <param name="json">A JSON string.</param>
		/// <returns>An ArrayList, a Hashtable, a float, a string, null, true, or false</returns>
		public static object JsonDecode(string json)
		{
			bool success = true;

			return JsonDecode(json, ref success);
		}

		/// <summary>
		/// Parses the string json into a value; and fills 'success' with the successfullness of the parse.
		/// </summary>
		/// <param name="json">A JSON string.</param>
		/// <param name="success">Successful parse?</param>
		/// <returns>An ArrayList, a Hashtable, a float, a string, null, true, or false</returns>
		public static object JsonDecode(string json, ref bool success)
		{
			success = true;
			if (json != null) {
				char[] charArray = json.ToCharArray();
				int index = 0;
				object value = ParseValue(charArray, ref index, ref success);
				return value;
			} else {
				return null;
			}
		}

		protected static Hashtable ParseObject(char[] json, ref int index, ref bool success)
		{
			Hashtable table = new Hashtable();
			int token;

			// {
			NextToken(json, ref index);

			bool done = false;
			while (!done) {
				token = LookAhead(json, index);
				if (token == JSON.TOKEN_NONE) {
					success = false;
					return null;
				} else if (token == JSON.TOKEN_COMMA) {
					NextToken(json, ref index);
				} else if (token == JSON.TOKEN_CURLY_CLOSE) {
					NextToken(json, ref index);
					return table;
				} else {

					// name
					string name = ParseString(json, ref index, ref success);
					if (!success) {
						success = false;
						return null;
					}

					// :
					token = NextToken(json, ref index);
					if (token != JSON.TOKEN_COLON) {
						success = false;
						return null;
					}

					// value
					object value = ParseValue(json, ref index, ref success);
					if (!success) {
						success = false;
						return null;
					}

					table[name] = value;
				}
			}

			return table;
		}

		protected static ArrayList ParseArray(char[] json, ref int index, ref bool success)
		{
			ArrayList array = new ArrayList();

			// [
			NextToken(json, ref index);

			bool done = false;
			while (!done) {
				int token = LookAhead(json, index);
				if (token == JSON.TOKEN_NONE) {
					success = false;
					return null;
				} else if (token == JSON.TOKEN_COMMA) {
					NextToken(json, ref index);
				} else if (token == JSON.TOKEN_SQUARED_CLOSE) {
					NextToken(json, ref index);
					break;
				} else {
					object value = ParseValue(json, ref index, ref success);
					if (!success) {
						return null;
					}

					array.Add(value);
				}
			}

			return array;
		}

		protected static object ParseValue(char[] json, ref int index, ref bool success)
		{
			switch (LookAhead(json, index)) {
				case JSON.TOKEN_STRING:
					return ParseString(json, ref index, ref success);
				case JSON.TOKEN_NUMBER:
					return ParseNumber(json, ref index, ref success);
				case JSON.TOKEN_CURLY_OPEN:
					return ParseObject(json, ref index, ref success);
				case JSON.TOKEN_SQUARED_OPEN:
					return ParseArray(json, ref index, ref success);
				case JSON.TOKEN_TRUE:
					NextToken(json, ref index);
					return true;
				case JSON.TOKEN_FALSE:
					NextToken(json, ref index);
					return false;
				case JSON.TOKEN_NULL:
					NextToken(json, ref index);
					return null;
				case JSON.TOKEN_NONE:
					break;
			}

			success = false;
			return null;
		}

		protected static string ParseString(char[] json, ref int index, ref bool success)
		{
			StringBuilder s = new StringBuilder(BUILDER_CAPACITY);
			char c;

			EatWhitespace(json, ref index);

			// "
			c = json[index++];

			bool complete = false;
			while (!complete) {

				if (index == json.Length) {
					break;
				}

				c = json[index++];
				if (c == '"') {
					complete = true;
					break;
				} else if (c == '\\') {

					if (index == json.Length) {
						break;
					}
					c = json[index++];
					if (c == '"') {
						s.Append('"');
					} else if (c == '\\') {
						s.Append('\\');
					} else if (c == '/') {
						s.Append('/');
					} else if (c == 'b') {
						s.Append('\b');
					} else if (c == 'f') {
						s.Append('\f');
					} else if (c == 'n') {
						s.Append('\n');
					} else if (c == 'r') {
						s.Append('\r');
					} else if (c == 't') {
						s.Append('\t');
					} else if (c == 'u') {
						int remainingLength = json.Length - index;
						if (remainingLength >= 4) {
							// parse the 32 bit hex into an integer codepoint
							uint codePoint;
							if (!(success = UInt32.TryParse(new string(json, index, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out codePoint))) {
								return "";
							}
							// convert the integer codepoint to a unicode char and add to string
							s.Append(Char.ConvertFromUtf32((int)codePoint));
							// skip 4 chars
							index += 4;
						} else {
							break;
						}
					}

				} else {
					s.Append(c);
				}

			}

			if (!complete) {
				success = false;
				return null;
			}

			return s.ToString();
		}

		protected static float ParseNumber(char[] json, ref int index, ref bool success)
		{
			EatWhitespace(json, ref index);

			int lastIndex = GetLastIndexOfNumber(json, index);
			int charLength = (lastIndex - index) + 1;

			double number;
			success = Double.TryParse(new string(json, index, charLength), NumberStyles.Any, CultureInfo.InvariantCulture, out number);
            float floatNum = (float)number;
			index = lastIndex + 1;
			return floatNum;
		}

		protected static int GetLastIndexOfNumber(char[] json, int index)
		{
			int lastIndex;

			for (lastIndex = index; lastIndex < json.Length; lastIndex++) {
				if ("0123456789+-.eE".IndexOf(json[lastIndex]) == -1) {
					break;
				}
			}
			return lastIndex - 1;
		}

		protected static void EatWhitespace(char[] json, ref int index)
		{
			for (; index < json.Length; index++) {
				if (" \t\n\r".IndexOf(json[index]) == -1) {
					break;
				}
			}
		}

		protected static int LookAhead(char[] json, int index)
		{
			int saveIndex = index;
			return NextToken(json, ref saveIndex);
		}

		protected static int NextToken(char[] json, ref int index)
		{
			EatWhitespace(json, ref index);

			if (index == json.Length) {
				return JSON.TOKEN_NONE;
			}

			char c = json[index];
			index++;
			switch (c) {
				case '{':
					return JSON.TOKEN_CURLY_OPEN;
				case '}':
					return JSON.TOKEN_CURLY_CLOSE;
				case '[':
					return JSON.TOKEN_SQUARED_OPEN;
				case ']':
					return JSON.TOKEN_SQUARED_CLOSE;
				case ',':
					return JSON.TOKEN_COMMA;
				case '"':
					return JSON.TOKEN_STRING;
				case '0': case '1': case '2': case '3': case '4':
				case '5': case '6': case '7': case '8': case '9':
				case '-':
					return JSON.TOKEN_NUMBER;
				case ':':
					return JSON.TOKEN_COLON;
			}
			index--;

			int remainingLength = json.Length - index;

			// false
			if (remainingLength >= 5) {
				if (json[index] == 'f' &&
					json[index + 1] == 'a' &&
					json[index + 2] == 'l' &&
					json[index + 3] == 's' &&
					json[index + 4] == 'e') {
					index += 5;
					return JSON.TOKEN_FALSE;
				}
			}

			// true
			if (remainingLength >= 4) {
				if (json[index] == 't' &&
					json[index + 1] == 'r' &&
					json[index + 2] == 'u' &&
					json[index + 3] == 'e') {
					index += 4;
					return JSON.TOKEN_TRUE;
				}
			}

			// null
			if (remainingLength >= 4) {
				if (json[index] == 'n' &&
					json[index + 1] == 'u' &&
					json[index + 2] == 'l' &&
					json[index + 3] == 'l') {
					index += 4;
					return JSON.TOKEN_NULL;
				}
			}

			return JSON.TOKEN_NONE;
		}


		protected static bool SerializeNumber(float number, StringBuilder builder)
		{
			builder.Append(Convert.ToString(number, CultureInfo.InvariantCulture));
			return true;
		}

	}
}

