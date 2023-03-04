using System.Collections.Generic;
using System.Linq;

namespace SmartShop.Utilities;

public static class AutoCorrect
{
	private static readonly Dictionary<char, char> _greekDictionary = new()
	{
		{ 'Ά', 'Α' },
		{ 'Έ', 'Ε' },
		{ 'Ή', 'Η' },
		{ 'Ί', 'Ι' },
		{ 'Ϊ', 'Ι' },
		{ 'Ό', 'Ο' },
		{ 'Ύ', 'Υ' },
		{ 'Ϋ', 'Υ' },
		{ 'Ώ', 'Ω' },
		{ 'ά', 'α' },
		{ 'έ', 'ε' },
		{ 'ή', 'η' },
		{ 'ί', 'ι' },
		{ 'ϊ', 'ι' },
		{ 'ΐ', 'ι' },
		{ 'ό', 'ο' },
		{ 'ύ', 'υ' },
		{ 'ϋ', 'υ' },
		{ 'ΰ', 'υ' },
		{ 'ώ', 'ω' }
	};

	public static string Normalize(this string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return s;
		}

		foreach (var c in s.Where(c => _greekDictionary.ContainsKey(c)))
		{
			s = s.Replace(c, _greekDictionary.GetValueOrDefault(c));
		}

		return s;
	}
}