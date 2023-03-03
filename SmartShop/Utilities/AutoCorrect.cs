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
		{ 'Ό', 'Ο' },
		{ 'Ύ', 'Υ' },
		{ 'Ώ', 'Ω' },
		{ 'ΐ', 'Ι' },
		{ 'Α', 'Α' },
		{ 'Β', 'Β' },
		{ 'Γ', 'Γ' },
		{ 'Δ', 'Δ' },
		{ 'Ε', 'Ε' },
		{ 'Ζ', 'Ζ' },
		{ 'Η', 'Η' },
		{ 'Θ', 'Θ' },
		{ 'Ι', 'Ι' },
		{ 'Κ', 'Κ' },
		{ 'Λ', 'Λ' },
		{ 'Μ', 'Μ' },
		{ 'Ν', 'Ν' },
		{ 'Ξ', 'Ξ' },
		{ 'Ο', 'Ο' },
		{ 'Π', 'Π' },
		{ 'Ρ', 'Ρ' },
		{ 'Σ', 'Σ' },
		{ 'Τ', 'Τ' },
		{ 'Υ', 'Υ' },
		{ 'Φ', 'Φ' },
		{ 'Χ', 'Χ' },
		{ 'Ψ', 'Ψ' },
		{ 'Ω', 'Ω' },
		{ 'Ϊ', 'Ι' },
		{ 'Ϋ', 'Υ' },
		{ 'ά', 'α' },
		{ 'έ', 'ε' },
		{ 'ή', 'η' },
		{ 'ί', 'ι' },
		{ 'ΰ', 'υ' },
		{ 'ύ', 'υ' },
		{ 'α', 'α' },
		{ 'β', 'β' },
		{ 'γ', 'γ' },
		{ 'δ', 'δ' },
		{ 'ε', 'ε' },
		{ 'ζ', 'ζ' },
		{ 'η', 'η' },
		{ 'θ', 'θ' },
		{ 'ι', 'ι' },
		{ 'κ', 'κ' },
		{ 'λ', 'λ' },
		{ 'μ', 'μ' },
		{ 'ν', 'ν' },
		{ 'ξ', 'ξ' },
		{ 'ο', 'ο' },
		{ 'ό', 'ο' },
		{ 'π', 'π' },
		{ 'ρ', 'ρ' },
		{ 'ς', 'ς' },
		{ 'σ', 'σ' },
		{ 'τ', 'τ' },
		{ 'υ', 'υ' },
		{ 'φ', 'φ' },
		{ 'χ', 'χ' },
		{ 'ψ', 'ψ' },
		{ 'ω', 'ω' }
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