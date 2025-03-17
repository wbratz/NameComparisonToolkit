using System.Text;

namespace NameComparisonToolkit.Similarity.Algos;

/*
 * Most of this implementation was lifted directly from the FuzzyStrings library
 * https://github.com/tylerjensen/FuzzyString
 */

internal static class DoubleMetaphoneExtensions
{
	public static bool CompareDoubleMetaphoneAdvanced(this string name1, string name2)
	{
		var codes1 = name1.GetDoubleMetaphoneExtended();
		var codes2 = name2.GetDoubleMetaphoneExtended();

		return string.Equals(codes1.Primary, codes2.Primary, StringComparison.OrdinalIgnoreCase) ||
			   string.Equals(codes1.Primary, codes2.Secondary, StringComparison.OrdinalIgnoreCase) ||
			   string.Equals(codes1.Secondary, codes2.Primary, StringComparison.OrdinalIgnoreCase) ||
			   string.Equals(codes1.Secondary, codes2.Secondary, StringComparison.OrdinalIgnoreCase);
	}

	internal static (string Primary, string Secondary) GetDoubleMetaphoneExtended(this string input)
	{
		var metaphoneData = new MetaphoneData();
		var current = 0;

		if (input.Length < 1)
		{
			return (string.Empty, string.Empty);
		}
		var last = input.Length - 1;

		var workingString = input.ToUpperInvariant() + "     ";

		var isSlavoGermanic = (input.IndexOf(charW) > -1)
			|| (input.IndexOf(charK) > -1)
			|| (input.IndexOf(strCZ, StringComparison.OrdinalIgnoreCase) > -1)
			|| (input.IndexOf(strWITZ, StringComparison.OrdinalIgnoreCase) > -1);

		if (workingString.StartsWith(StringComparison.OrdinalIgnoreCase, strGN, strKN, strPN, strWR, strPS))
		{
			current++;
		}

		if (workingString[0] == charX)
		{
			metaphoneData.Add(strS); //'Z' maps to 'S'
			current++;
		}

		while ((metaphoneData.PrimaryLength < 4) || (metaphoneData.SecondaryLength < 4))
		{
			if (current >= input.Length)
				break;

			switch (workingString[current])
			{
				case charA:
				case charE:
				case charI:
				case charO:
				case charU:
				case charY:
					if (current == 0)
					{
						//all init vowels now map to 'A'
						metaphoneData.Add("A");
					}
					current++;
					break;

				case charB:
					//"-mb", e.g", "dumb", already skipped over...
					metaphoneData.Add("P");

					if (workingString[current + 1] == charB)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					break;

				case charAdash:
					metaphoneData.Add(strS);
					current++;
					break;

				case charC:
					//various germanic
					if ((current > 1)
						&& !IsVowel(workingString[current - 2])
						&& StringAt(workingString, (current - 1), strACH)
						&& ((workingString[current + 2] != charI)
						&& ((workingString[current + 2] != charE)
						|| StringAt(workingString, (current - 2), strBACHER, strMACHER))))
					{
						metaphoneData.Add(strK);
						current += 2;
						break;
					}

					//special case 'caesar'
					if ((current == 0) && StringAt(workingString, current, strCAESAR))
					{
						metaphoneData.Add(strS);
						current += 2;
						break;
					}

					//italian 'chianti'
					if (StringAt(workingString, current, strCHIA))
					{
						metaphoneData.Add(strK);
						current += 2;
						break;
					}

					if (StringAt(workingString, current, strCH))
					{
						//find 'michael'
						if ((current > 0) && StringAt(workingString, current, strCHAE))
						{
							metaphoneData.Add(strK, strX);
							current += 2;
							break;
						}

						//greek roots e.g. 'chemistry', 'chorus'
						if ((current == 0)
							&& (StringAt(workingString, (current + 1), strHARAC, strHARIS)
							|| StringAt(workingString, (current + 1), strHOR, strHYM, strHIA, strHEM))
							&& !StringAt(workingString, 0, strCHORE))
						{
							metaphoneData.Add(strK);
							current += 2;
							break;
						}

						//germanic, greek, or otherwise 'ch' for 'kh' sound
						if ((StringAt(workingString, 0, strVANsp, strVONsp)
							|| StringAt(workingString, 0, strSCH)) // 'architect but not 'arch', 'orchestra', 'orchid'
							|| StringAt(workingString, (current - 2), strORCHES, strARCHIT, strORCHID)
							|| StringAt(workingString, (current + 2), strT, strS)
								|| ((StringAt(workingString, (current - 1), strA, strO, strU, strE)
								|| (current == 0)) //e.g., 'wachtler', 'wechsler', but not 'tichner'
									&& StringAt(workingString, (current + 2), strL, strR, strN, strM, strB, strH, strF, strV, strW, sp)))
						{
							metaphoneData.Add(strK);
						}
						else
						{
							if (current > 0)
							{
								if (StringAt(workingString, 0, strMC))
								{
									//e.g., "McHugh"
									metaphoneData.Add(strK);
								}
								else
								{
									metaphoneData.Add(strX, strK);
								}
							}
							else
							{
								metaphoneData.Add(strX);
							}
						}
						current += 2;
						break;
					}
					//e.g, 'czerny'
					if (StringAt(workingString, current, strCZ) && !StringAt(workingString, (current - 2), strWICZ))
					{
						metaphoneData.Add(strS, strX);
						current += 2;
						break;
					}

					//e.g., 'focaccia'
					if (StringAt(workingString, (current + 1), strCIA))
					{
						metaphoneData.Add(strX);
						current += 3;
						break;
					}

					//double 'C', but not if e.g. 'McClellan'
					if (StringAt(workingString, current, strCC) && !((current == 1) && (workingString[0] == charM)))
					{
						//'bellocchio' but not 'bacchus'
						if (StringAt(workingString, (current + 2), strI, strE, strH)
							&& !StringAt(workingString, (current + 2), strHU))
						{
							//'accident', 'accede' 'succeed'
							if (((current == 1) && (workingString[current - 1] == charA))
								|| StringAt(workingString, (current - 1), strUCCEE, strUCCES))
							{
								metaphoneData.Add(strKS);
							}
							//'bacci', 'bertucci', other italian
							else
							{
								metaphoneData.Add(strX);
							}
							current += 3;
							break;
						}
						else
						{
							//Pierce's rule
							metaphoneData.Add(strK);
							current += 2;
							break;
						}
					}

					if (StringAt(workingString, current, strCK, strCG, strCQ))
					{
						metaphoneData.Add(strK);
						current += 2;
						break;
					}

					if (StringAt(workingString, current, strCI, strCE, strCY))
					{
						//italian vs. english
						if (StringAt(workingString, current, strCIO, strCIE, strCIA))
						{
							metaphoneData.Add(strS, strX);
						}
						else
						{
							metaphoneData.Add(strS);
						}
						current += 2;
						break;
					}

					//else
					metaphoneData.Add(strK);

					//name sent in 'mac caffrey', 'mac gregor
					if (StringAt(workingString, (current + 1), strspC, strspQ, strspG))
					{
						current += 3;
					}
					else if (StringAt(workingString, (current + 1), strC, strK, strQ)
						&& !StringAt(workingString, (current + 1), strCE, strCI))
					{
						current += 2;
					}
					else
					{
						current++;
					}
					break;

				case charD:
					if (StringAt(workingString, current, strDG))
					{
						if (StringAt(workingString, (current + 2), strI, strE, strY))
						{
							//e.g. 'edge'
							metaphoneData.Add(strJ);
							current += 3;
							break;
						}
						else
						{
							//e.g. 'edgar'
							metaphoneData.Add(strTK);
							current += 2;
							break;
						}
					}

					if (StringAt(workingString, current, strDT, strDD))
					{
						metaphoneData.Add(strT);
						current += 2;
						break;
					}

					//else
					metaphoneData.Add(strT);
					current++;
					break;

				case charF:
					if (workingString[current + 1] == charF)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add(strF);
					break;

				case charG:
					if (workingString[current + 1] == charH)
					{
						if ((current > 0) && !IsVowel(workingString[current - 1]))
						{
							metaphoneData.Add(strK);
							current += 2;
							break;
						}

						if (current < 3)
						{
							//'ghislane', ghiradelli
							if (current == 0)
							{
								if (workingString[current + 2] == charI)
								{
									metaphoneData.Add(strJ);
								}
								else
								{
									metaphoneData.Add(strK);
								}
								current += 2;
								break;
							}
						}
						//Parker's rule (with some further refinements) - e.g., 'hugh'
						if (((current > 1) && StringAt(workingString, (current - 2), strB, strH, strD)) //e.g., 'bough'
							|| ((current > 2) && StringAt(workingString, (current - 3), strB, strH, strD)) //e.g., 'broughton'
								|| ((current > 3) && StringAt(workingString, (current - 4), strB, strH)))
						{
							current += 2;
							break;
						}
						else
						{
							//e.g., 'laugh', 'McLaughlin', 'cough', 'gough', 'rough', 'tough'
							if ((current > 2) && (workingString[current - 1] == charU)
								&& StringAt(workingString, (current - 3), strC, strG, strL, strR, strT))
							{
								metaphoneData.Add(strF);
							}
							else if ((current > 0) && workingString[current - 1] != charI)
							{
								metaphoneData.Add(strK);
							}

							current += 2;
							break;
						}
					}

					if (workingString[current + 1] == charN)
					{
						if ((current == 1) && IsVowel(workingString[0]) && !isSlavoGermanic)
						{
							metaphoneData.Add(strKN, strN);
						}
						else
							//not e.g. 'cagney'
							if (!StringAt(workingString, (current + 2), strEY)
								&& (workingString[current + 1] != charY) && !isSlavoGermanic)
						{
							metaphoneData.Add(strN, strKN);
						}
						else
						{
							metaphoneData.Add(strKN);
						}
						current += 2;
						break;
					}

					//'tagliaro'
					if (StringAt(workingString, (current + 1), strLI) && !isSlavoGermanic)
					{
						metaphoneData.Add(strKL, strL);
						current += 2;
						break;
					}

					//-ges-,-gep-,-gel-, -gie- at beginning
					if ((current == 0)
						&& ((workingString[current + 1] == charY)
						|| StringAt(workingString, (current + 1), strES, strEP, strEB, strEL, strEY, strIB, strIL, strIN, strIE, strEI, strER)))
					{
						metaphoneData.Add(strK, strJ);
						current += 2;
						break;
					}

					// -ger-,  -gy-
					if ((StringAt(workingString, (current + 1), strER)
						|| (workingString[current + 1] == charY))
						&& !StringAt(workingString, 0, strDANGER, strRANGER, strMANGER)
						&& !StringAt(workingString, (current - 1), strE, strI)
						&& !StringAt(workingString, (current - 1), strRGY, strOGY))
					{
						metaphoneData.Add(strK, strJ);
						current += 2;
						break;
					}

					// italian e.g, 'biaggi'
					if (StringAt(workingString, (current + 1), strE, strI, strY)
						|| StringAt(workingString, (current - 1), strAGGI, strOGGI))
					{
						//obvious germanic
						if ((StringAt(workingString, 0, strVANsp, strVONsp)
							|| StringAt(workingString, 0, strSCH))
							|| StringAt(workingString, (current + 1), strET))
						{
							metaphoneData.Add(strK);
						}
						else
							//always soft if french ending
							if (StringAt(workingString, (current + 1), strIERsp))
						{
							metaphoneData.Add(strJ);
						}
						else
						{
							metaphoneData.Add(strJ, strK);
						}
						current += 2;
						break;
					}

					if (workingString[current + 1] == charG)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add(strK);
					break;

				case 'H':
					//only keep if first & before vowel or btw. 2 vowels
					if (((current == 0) || IsVowel(workingString[current - 1])) && IsVowel(workingString[current + 1]))
					{
						metaphoneData.Add(strH);
						current += 2;
					}
					else //also takes care of 'HH'
					{
						current++;
					}
					break;

				case 'J':
					//obvious spanish, 'jose', 'san jacinto'
					if (StringAt(workingString, current, strJOSE) || StringAt(workingString, 0, strSANsp))
					{
						if (((current == 0) && (workingString[current + 4] == ' ')) || StringAt(workingString, 0, strSANsp))
						{
							metaphoneData.Add(strH);
						}
						else
						{
							metaphoneData.Add(strJ, strH);
						}
						current++;
						break;
					}

					if ((current == 0) && !StringAt(workingString, current, strJOSE))
					{
						metaphoneData.Add(strJ, strA); //Yankelovich/Jankelowicz
					}
					else
						//spanish pron. of e.g. 'bajador'
						if (current > 0
							&& IsVowel(workingString[current - 1])
							&& !isSlavoGermanic && ((workingString[current + 1] == charA)
							|| (workingString[current + 1] == charO)))
					{
						metaphoneData.Add(strJ, strH);
					}
					else if (current == last)
					{
						metaphoneData.Add(strJ, sp);
					}
					else if (!StringAt(workingString, (current + 1), strL, strT, strK, strS, strN, strM, strB, strZ)
						&& !StringAt(workingString, (current - 1), strS, strK, strL))
					{
						metaphoneData.Add(strJ);
					}

					if (workingString[current + 1] == charJ) //it could happen!
					{
						current += 2;
					}
					else
					{
						current++;
					}
					break;

				case charK:
					if (workingString[current + 1] == charK)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add(strK);
					break;

				case charL:
					if (workingString[current + 1] == charL)
					{
						//spanish e.g. 'cabrillo', 'gallegos'
						if (((current == (input.Length - 3))
							&& StringAt(workingString, (current - 1), strILLO, strILLA, strALLE))
							|| ((StringAt(workingString, (last - 1), strAS, strOS)
							|| StringAt(workingString, last, strA, strO))
							&& StringAt(workingString, (current - 1), strALLE)))
						{
							metaphoneData.Add(strL, sp);
							current += 2;
							break;
						}
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add("L");
					break;

				case charM:
					if ((StringAt(workingString, (current - 1), strUMB)
						&& (((current + 1) == last)
						|| StringAt(workingString, (current + 2), strER))) //'dumb','thumb'
						|| (workingString[current + 1] == charM))
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add("M");
					break;

				case charN:
					if (workingString[current + 1] == charN)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add(strN);
					break;

				case charOdash:
					current++;
					metaphoneData.Add(strN);
					break;

				case charP:
					if (workingString[current + 1] == charH)
					{
						metaphoneData.Add(strF);
						current += 2;
						break;
					}

					//also account for "campbell", "raspberry"
					if (StringAt(workingString, (current + 1), strP, strB))
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add(strP);
					break;

				case charQ:
					if (workingString[current + 1] == charQ)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add(strK);
					break;

				case charR:
					//french e.g. 'rogier', but exclude 'hochmeier'
					if ((current == last) && !isSlavoGermanic
						&& StringAt(workingString, (current - 2), strIE)
						&& !StringAt(workingString, (current - 4), strME, strMA))
					{
						metaphoneData.Add(string.Empty, strR);
					}
					else
					{
						metaphoneData.Add(strR);
					}

					if (workingString[current + 1] == charR)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					break;

				case charS:
					//special cases 'island', 'isle', 'carlisle', 'carlysle'
					if (StringAt(workingString, (current - 1), strISL, strYSL))
					{
						current++;
						break;
					}

					//special case 'sugar-'
					if ((current == 0) && StringAt(workingString, current, strSUGAR))
					{
						metaphoneData.Add(strX, strS);
						current++;
						break;
					}

					if (StringAt(workingString, current, strSH))
					{
						//germanic
						if (StringAt(workingString, (current + 1), strHEIM, strHOEK, strHOLM, strHOLZ))
						{
							metaphoneData.Add(strS);
						}
						else
						{
							metaphoneData.Add(strX);
						}
						current += 2;
						break;
					}

					//italian & armenian
					if (StringAt(workingString, current, strSIO, strSIA) || StringAt(workingString, current, strSIAN))
					{
						if (!isSlavoGermanic)
						{
							metaphoneData.Add(strS, strX);
						}
						else
						{
							metaphoneData.Add(strS);
						}
						current += 3;
						break;
					}

					//german & anglicisations, e.g. 'smith' match 'schmidt', 'snider' match 'schneider'
					//also, -sz- in slavic language altho in hungarian it is pronounced 's'
					if (((current == 0)
						&& StringAt(workingString, (current + 1), strM, strN, strL, strW))
						|| StringAt(workingString, (current + 1), strZ))
					{
						metaphoneData.Add(strS, strX);
						if (StringAt(workingString, (current + 1), strZ))
						{
							current += 2;
						}
						else
						{
							current++;
						}
						break;
					}

					if (StringAt(workingString, current, strSC))
					{
						//Schlesinger's rule
						if (workingString[current + 2] == charH)
						{
							//dutch origin, e.g. 'school', 'schooner'
							if (StringAt(workingString, (current + 3), strOO, strER, strEN, strUY, strED, strEM))
							{
								//'schermerhorn', 'schenker'
								if (StringAt(workingString, (current + 3), strER, strEN))
								{
									metaphoneData.Add(strX, strSK);
								}
								else
								{
									metaphoneData.Add(strSK);
								}
								current += 3;
								break;
							}
							else
							{
								if ((current == 0) && !IsVowel(workingString[3]) && (workingString[3] != charW))
								{
									metaphoneData.Add(strX, strS);
								}
								else
								{
									metaphoneData.Add(strX);
								}
								current += 3;
								break;
							}
						}

						if (StringAt(workingString, (current + 2), strI, strE, strY))
						{
							metaphoneData.Add(strS);
							current += 3;
							break;
						}
						//else
						metaphoneData.Add(strSK);
						current += 3;
						break;
					}

					//french e.g. 'resnais', 'artois'
					if ((current == last) && StringAt(workingString, (current - 2), strAI, strOI))
					{
						metaphoneData.Add(string.Empty, strS);
					}
					else
					{
						metaphoneData.Add(strS);
					}

					if (StringAt(workingString, (current + 1), strS, strZ))
					{
						current += 2;
					}
					else
					{
						current++;
					}
					break;

				case charT:
					if (StringAt(workingString, current, strTION))
					{
						metaphoneData.Add(strX);
						current += 3;
						break;
					}

					if (StringAt(workingString, current, strTIA, strTCH))
					{
						metaphoneData.Add(strX);
						current += 3;
						break;
					}

					if (StringAt(workingString, current, strTH) || StringAt(workingString, current, strTTH))
					{
						//special case 'thomas', 'thames' or germanic
						if (StringAt(workingString, (current + 2), strOM, strAM)
							|| StringAt(workingString, 0, strVANsp, strVONsp) || StringAt(workingString, 0, strSCH))
						{
							metaphoneData.Add(strT);
						}
						else
						{
							metaphoneData.Add(strO, strT);
						}
						current += 2;
						break;
					}

					if (StringAt(workingString, (current + 1), strT, strD))
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add(strT);
					break;

				case charV:
					if (workingString[current + 1] == charV)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add(strF);
					break;

				case charW:
					//can also be in middle of word
					if (StringAt(workingString, current, strWR))
					{
						metaphoneData.Add(strR);
						current += 2;
						break;
					}

					if ((current == 0) && (IsVowel(workingString[current + 1])
						|| StringAt(workingString, current, strWH)))
					{
						//Wasserman should match Vasserman
						if (IsVowel(workingString[current + 1]))
						{
							metaphoneData.Add(strA, strF);
						}
						else
						{
							//need Uomo to match Womo
							metaphoneData.Add(strA);
						}
					}

					//Arnow should match Arnoff
					if ((current == last && current > 0 && IsVowel(workingString[current - 1]))
						|| StringAt(workingString, (current - 1), strEWSKI, strEWSKY, strOWSKI, strOWSKY)
						|| StringAt(workingString, 0, strSCH))
					{
						metaphoneData.Add(string.Empty, strF);
						current++;
						break;
					}

					//polish e.g. 'filipowicz'
					if (StringAt(workingString, current, strWICZ, strWITZ))
					{
						metaphoneData.Add(strTS, strFX);
						current += 4;
						break;
					}

					//else skip it
					current++;
					break;

				case charX:
					//french e.g. breaux
					if (!((current == last)
						&& (StringAt(workingString, (current - 3), strIAU, strEAU)
						|| StringAt(workingString, (current - 2), strAU, strOU))))
					{
						metaphoneData.Add(strKS);
					}

					if (StringAt(workingString, (current + 1), strC, strX))
					{
						current += 2;
					}
					else
					{
						current++;
					}
					break;

				case charZ:
					//chinese pinyin e.g. 'zhao'
					if (workingString[current + 1] == charH)
					{
						metaphoneData.Add(strJ);
						current += 2;
						break;
					}
					else if (StringAt(workingString, (current + 1), strZO, strZI, strZA)
						|| (isSlavoGermanic && ((current > 0) && workingString[current - 1] != charT)))
					{
						metaphoneData.Add(strS, strTS);
					}
					else
					{
						metaphoneData.Add(strS);
					}

					if (workingString[current + 1] == charZ)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					break;

				default:
					current++;
					break;
			}
		}

		return metaphoneData.GetCodes();
	}

	internal static string ToDoubleMetaphone(this string input)
	{
		var metaphoneData = new MetaphoneData();
		var current = 0;

		if (input.Length < 1)
		{
			return input;
		}
		var last = input.Length - 1; //zero based index

		var workingString = input.ToUpperInvariant() + "     ";

		var isSlavoGermanic = (input.IndexOf(charW) > -1)
			|| (input.IndexOf(charK) > -1)
			|| (input.IndexOf(strCZ, StringComparison.OrdinalIgnoreCase) > -1)
			|| (input.IndexOf(strWITZ, StringComparison.OrdinalIgnoreCase) > -1);

		//skip these when at start of word
		if (workingString.StartsWith(StringComparison.OrdinalIgnoreCase, strGN, strKN, strPN, strWR, strPS))
		{
			current++;
		}

		//Initial 'X' is pronounced 'Z' e.g. 'Xavier'
		if (workingString[0] == charX)
		{
			metaphoneData.Add(strS); //'Z' maps to 'S'
			current++;
		}

		while ((metaphoneData.PrimaryLength < 4) || (metaphoneData.SecondaryLength < 4))
		{
			if (current >= input.Length)
			{
				break;
			}

			switch (workingString[current])
			{
				case charA:
				case charE:
				case charI:
				case charO:
				case charU:
				case charY:
					if (current == 0)
					{
						//all init vowels now map to 'A'
						metaphoneData.Add("A");
					}
					current++;
					break;

				case charB:
					//"-mb", e.g", "dumb", already skipped over...
					metaphoneData.Add("P");

					if (workingString[current + 1] == charB)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					break;

				case charAdash:
					metaphoneData.Add(strS);
					current++;
					break;

				case charC:
					//various germanic
					if ((current > 1)
						&& !IsVowel(workingString[current - 2])
						&& StringAt(workingString, (current - 1), strACH)
						&& ((workingString[current + 2] != charI)
						&& ((workingString[current + 2] != charE)
						|| StringAt(workingString, (current - 2), strBACHER, strMACHER))))
					{
						metaphoneData.Add(strK);
						current += 2;
						break;
					}

					//special case 'caesar'
					if ((current == 0) && StringAt(workingString, current, strCAESAR))
					{
						metaphoneData.Add(strS);
						current += 2;
						break;
					}

					//italian 'chianti'
					if (StringAt(workingString, current, strCHIA))
					{
						metaphoneData.Add(strK);
						current += 2;
						break;
					}

					if (StringAt(workingString, current, strCH))
					{
						//find 'michael'
						if ((current > 0) && StringAt(workingString, current, strCHAE))
						{
							metaphoneData.Add(strK, strX);
							current += 2;
							break;
						}

						//greek roots e.g. 'chemistry', 'chorus'
						if ((current == 0)
							&& (StringAt(workingString, (current + 1), strHARAC, strHARIS)
							|| StringAt(workingString, (current + 1), strHOR, strHYM, strHIA, strHEM))
							&& !StringAt(workingString, 0, strCHORE))
						{
							metaphoneData.Add(strK);
							current += 2;
							break;
						}

						//germanic, greek, or otherwise 'ch' for 'kh' sound
						if ((StringAt(workingString, 0, strVANsp, strVONsp)
							|| StringAt(workingString, 0, strSCH)) // 'architect but not 'arch', 'orchestra', 'orchid'
							|| StringAt(workingString, (current - 2), strORCHES, strARCHIT, strORCHID)
							|| StringAt(workingString, (current + 2), strT, strS)
								|| ((StringAt(workingString, (current - 1), strA, strO, strU, strE)
								|| (current == 0)) //e.g., 'wachtler', 'wechsler', but not 'tichner'
									&& StringAt(workingString, (current + 2), strL, strR, strN, strM, strB, strH, strF, strV, strW, sp)))
						{
							metaphoneData.Add(strK);
						}
						else
						{
							if (current > 0)
							{
								if (StringAt(workingString, 0, strMC))
								{
									//e.g., "McHugh"
									metaphoneData.Add(strK);
								}
								else
								{
									metaphoneData.Add(strX, strK);
								}
							}
							else
							{
								metaphoneData.Add(strX);
							}
						}
						current += 2;
						break;
					}
					//e.g, 'czerny'
					if (StringAt(workingString, current, strCZ) && !StringAt(workingString, (current - 2), strWICZ))
					{
						metaphoneData.Add(strS, strX);
						current += 2;
						break;
					}

					//e.g., 'focaccia'
					if (StringAt(workingString, (current + 1), strCIA))
					{
						metaphoneData.Add(strX);
						current += 3;
						break;
					}

					//double 'C', but not if e.g. 'McClellan'
					if (StringAt(workingString, current, strCC) && !((current == 1) && (workingString[0] == charM)))
					{
						//'bellocchio' but not 'bacchus'
						if (StringAt(workingString, (current + 2), strI, strE, strH)
							&& !StringAt(workingString, (current + 2), strHU))
						{
							//'accident', 'accede' 'succeed'
							if (((current == 1) && (workingString[current - 1] == charA))
								|| StringAt(workingString, (current - 1), strUCCEE, strUCCES))
							{
								metaphoneData.Add(strKS);
							}
							//'bacci', 'bertucci', other italian
							else
							{
								metaphoneData.Add(strX);
							}
							current += 3;
							break;
						}
						else
						{
							//Pierce's rule
							metaphoneData.Add(strK);
							current += 2;
							break;
						}
					}

					if (StringAt(workingString, current, strCK, strCG, strCQ))
					{
						metaphoneData.Add(strK);
						current += 2;
						break;
					}

					if (StringAt(workingString, current, strCI, strCE, strCY))
					{
						//italian vs. english
						if (StringAt(workingString, current, strCIO, strCIE, strCIA))
						{
							metaphoneData.Add(strS, strX);
						}
						else
						{
							metaphoneData.Add(strS);
						}
						current += 2;
						break;
					}

					//else
					metaphoneData.Add(strK);

					//name sent in 'mac caffrey', 'mac gregor
					if (StringAt(workingString, (current + 1), strspC, strspQ, strspG))
					{
						current += 3;
					}
					else if (StringAt(workingString, (current + 1), strC, strK, strQ)
						&& !StringAt(workingString, (current + 1), strCE, strCI))
					{
						current += 2;
					}
					else
					{
						current++;
					}
					break;

				case charD:
					if (StringAt(workingString, current, strDG))
					{
						if (StringAt(workingString, (current + 2), strI, strE, strY))
						{
							//e.g. 'edge'
							metaphoneData.Add(strJ);
							current += 3;
							break;
						}
						else
						{
							//e.g. 'edgar'
							metaphoneData.Add(strTK);
							current += 2;
							break;
						}
					}

					if (StringAt(workingString, current, strDT, strDD))
					{
						metaphoneData.Add(strT);
						current += 2;
						break;
					}

					//else
					metaphoneData.Add(strT);
					current++;
					break;

				case charF:
					if (workingString[current + 1] == charF)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add(strF);
					break;

				case charG:
					if (workingString[current + 1] == charH)
					{
						if ((current > 0) && !IsVowel(workingString[current - 1]))
						{
							metaphoneData.Add(strK);
							current += 2;
							break;
						}

						if (current < 3)
						{
							//'ghislane', ghiradelli
							if (current == 0)
							{
								if (workingString[current + 2] == charI)
								{
									metaphoneData.Add(strJ);
								}
								else
								{
									metaphoneData.Add(strK);
								}
								current += 2;
								break;
							}
						}
						//Parker's rule (with some further refinements) - e.g., 'hugh'
						if (((current > 1) && StringAt(workingString, (current - 2), strB, strH, strD)) //e.g., 'bough'
							|| ((current > 2) && StringAt(workingString, (current - 3), strB, strH, strD)) //e.g., 'broughton'
								|| ((current > 3) && StringAt(workingString, (current - 4), strB, strH)))
						{
							current += 2;
							break;
						}
						else
						{
							//e.g., 'laugh', 'McLaughlin', 'cough', 'gough', 'rough', 'tough'
							if ((current > 2) && (workingString[current - 1] == charU)
								&& StringAt(workingString, (current - 3), strC, strG, strL, strR, strT))
							{
								metaphoneData.Add(strF);
							}
							else if ((current > 0) && workingString[current - 1] != charI)
							{
								metaphoneData.Add(strK);
							}

							current += 2;
							break;
						}
					}

					if (workingString[current + 1] == charN)
					{
						if ((current == 1) && IsVowel(workingString[0]) && !isSlavoGermanic)
						{
							metaphoneData.Add(strKN, strN);
						}
						else
							//not e.g. 'cagney'
							if (!StringAt(workingString, (current + 2), strEY)
								&& (workingString[current + 1] != charY) && !isSlavoGermanic)
						{
							metaphoneData.Add(strN, strKN);
						}
						else
						{
							metaphoneData.Add(strKN);
						}
						current += 2;
						break;
					}

					//'tagliaro'
					if (StringAt(workingString, (current + 1), strLI) && !isSlavoGermanic)
					{
						metaphoneData.Add(strKL, strL);
						current += 2;
						break;
					}

					//-ges-,-gep-,-gel-, -gie- at beginning
					if ((current == 0)
						&& ((workingString[current + 1] == charY)
						|| StringAt(workingString, (current + 1), strES, strEP, strEB, strEL, strEY, strIB, strIL, strIN, strIE, strEI, strER)))
					{
						metaphoneData.Add(strK, strJ);
						current += 2;
						break;
					}

					// -ger-,  -gy-
					if ((StringAt(workingString, (current + 1), strER)
						|| (workingString[current + 1] == charY))
						&& !StringAt(workingString, 0, strDANGER, strRANGER, strMANGER)
						&& !StringAt(workingString, (current - 1), strE, strI)
						&& !StringAt(workingString, (current - 1), strRGY, strOGY))
					{
						metaphoneData.Add(strK, strJ);
						current += 2;
						break;
					}

					// italian e.g, 'biaggi'
					if (StringAt(workingString, (current + 1), strE, strI, strY)
						|| StringAt(workingString, (current - 1), strAGGI, strOGGI))
					{
						//obvious germanic
						if ((StringAt(workingString, 0, strVANsp, strVONsp)
							|| StringAt(workingString, 0, strSCH))
							|| StringAt(workingString, (current + 1), strET))
						{
							metaphoneData.Add(strK);
						}
						else
							//always soft if french ending
							if (StringAt(workingString, (current + 1), strIERsp))
						{
							metaphoneData.Add(strJ);
						}
						else
						{
							metaphoneData.Add(strJ, strK);
						}
						current += 2;
						break;
					}

					if (workingString[current + 1] == charG)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add(strK);
					break;

				case 'H':
					//only keep if first & before vowel or btw. 2 vowels
					if (((current == 0) || IsVowel(workingString[current - 1])) && IsVowel(workingString[current + 1]))
					{
						metaphoneData.Add(strH);
						current += 2;
					}
					else //also takes care of 'HH'
					{
						current++;
					}
					break;

				case 'J':
					//obvious spanish, 'jose', 'san jacinto'
					if (StringAt(workingString, current, strJOSE) || StringAt(workingString, 0, strSANsp))
					{
						if (((current == 0) && (workingString[current + 4] == ' ')) || StringAt(workingString, 0, strSANsp))
						{
							metaphoneData.Add(strH);
						}
						else
						{
							metaphoneData.Add(strJ, strH);
						}
						current++;
						break;
					}

					if ((current == 0) && !StringAt(workingString, current, strJOSE))
					{
						metaphoneData.Add(strJ, strA); //Yankelovich/Jankelowicz
					}
					else
						//spanish pron. of e.g. 'bajador'
						if (current > 0
							&& IsVowel(workingString[current - 1])
							&& !isSlavoGermanic && ((workingString[current + 1] == charA)
							|| (workingString[current + 1] == charO)))
					{
						metaphoneData.Add(strJ, strH);
					}
					else if (current == last)
					{
						metaphoneData.Add(strJ, sp);
					}
					else if (!StringAt(workingString, (current + 1), strL, strT, strK, strS, strN, strM, strB, strZ)
						&& !StringAt(workingString, (current - 1), strS, strK, strL))
					{
						metaphoneData.Add(strJ);
					}

					if (workingString[current + 1] == charJ) //it could happen!
					{
						current += 2;
					}
					else
					{
						current++;
					}
					break;

				case charK:
					if (workingString[current + 1] == charK)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add(strK);
					break;

				case charL:
					if (workingString[current + 1] == charL)
					{
						//spanish e.g. 'cabrillo', 'gallegos'
						if (((current == (input.Length - 3))
							&& StringAt(workingString, (current - 1), strILLO, strILLA, strALLE))
							|| ((StringAt(workingString, (last - 1), strAS, strOS)
							|| StringAt(workingString, last, strA, strO))
							&& StringAt(workingString, (current - 1), strALLE)))
						{
							metaphoneData.Add(strL, sp);
							current += 2;
							break;
						}
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add("L");
					break;

				case charM:
					if ((StringAt(workingString, (current - 1), strUMB)
						&& (((current + 1) == last)
						|| StringAt(workingString, (current + 2), strER))) //'dumb','thumb'
						|| (workingString[current + 1] == charM))
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add("M");
					break;

				case charN:
					if (workingString[current + 1] == charN)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add(strN);
					break;

				case charOdash:
					current++;
					metaphoneData.Add(strN);
					break;

				case charP:
					if (workingString[current + 1] == charH)
					{
						metaphoneData.Add(strF);
						current += 2;
						break;
					}

					//also account for "campbell", "raspberry"
					if (StringAt(workingString, (current + 1), strP, strB))
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add(strP);
					break;

				case charQ:
					if (workingString[current + 1] == charQ)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add(strK);
					break;

				case charR:
					//french e.g. 'rogier', but exclude 'hochmeier'
					if ((current == last) && !isSlavoGermanic
						&& StringAt(workingString, (current - 2), strIE)
						&& !StringAt(workingString, (current - 4), strME, strMA))
					{
						metaphoneData.Add(string.Empty, strR);
					}
					else
					{
						metaphoneData.Add(strR);
					}

					if (workingString[current + 1] == charR)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					break;

				case charS:
					//special cases 'island', 'isle', 'carlisle', 'carlysle'
					if (StringAt(workingString, (current - 1), strISL, strYSL))
					{
						current++;
						break;
					}

					//special case 'sugar-'
					if ((current == 0) && StringAt(workingString, current, strSUGAR))
					{
						metaphoneData.Add(strX, strS);
						current++;
						break;
					}

					if (StringAt(workingString, current, strSH))
					{
						//germanic
						if (StringAt(workingString, (current + 1), strHEIM, strHOEK, strHOLM, strHOLZ))
						{
							metaphoneData.Add(strS);
						}
						else
						{
							metaphoneData.Add(strX);
						}
						current += 2;
						break;
					}

					//italian & armenian
					if (StringAt(workingString, current, strSIO, strSIA) || StringAt(workingString, current, strSIAN))
					{
						if (!isSlavoGermanic)
						{
							metaphoneData.Add(strS, strX);
						}
						else
						{
							metaphoneData.Add(strS);
						}
						current += 3;
						break;
					}

					//german & anglicisations, e.g. 'smith' match 'schmidt', 'snider' match 'schneider'
					//also, -sz- in slavic language altho in hungarian it is pronounced 's'
					if (((current == 0)
						&& StringAt(workingString, (current + 1), strM, strN, strL, strW))
						|| StringAt(workingString, (current + 1), strZ))
					{
						metaphoneData.Add(strS, strX);
						if (StringAt(workingString, (current + 1), strZ))
						{
							current += 2;
						}
						else
						{
							current++;
						}
						break;
					}

					if (StringAt(workingString, current, strSC))
					{
						//Schlesinger's rule
						if (workingString[current + 2] == charH)
						{
							//dutch origin, e.g. 'school', 'schooner'
							if (StringAt(workingString, (current + 3), strOO, strER, strEN, strUY, strED, strEM))
							{
								//'schermerhorn', 'schenker'
								if (StringAt(workingString, (current + 3), strER, strEN))
								{
									metaphoneData.Add(strX, strSK);
								}
								else
								{
									metaphoneData.Add(strSK);
								}
								current += 3;
								break;
							}
							else
							{
								if ((current == 0) && !IsVowel(workingString[3]) && (workingString[3] != charW))
								{
									metaphoneData.Add(strX, strS);
								}
								else
								{
									metaphoneData.Add(strX);
								}
								current += 3;
								break;
							}
						}

						if (StringAt(workingString, (current + 2), strI, strE, strY))
						{
							metaphoneData.Add(strS);
							current += 3;
							break;
						}
						//else
						metaphoneData.Add(strSK);
						current += 3;
						break;
					}

					//french e.g. 'resnais', 'artois'
					if ((current == last) && StringAt(workingString, (current - 2), strAI, strOI))
					{
						metaphoneData.Add(string.Empty, strS);
					}
					else
					{
						metaphoneData.Add(strS);
					}

					if (StringAt(workingString, (current + 1), strS, strZ))
					{
						current += 2;
					}
					else
					{
						current++;
					}
					break;

				case charT:
					if (StringAt(workingString, current, strTION))
					{
						metaphoneData.Add(strX);
						current += 3;
						break;
					}

					if (StringAt(workingString, current, strTIA, strTCH))
					{
						metaphoneData.Add(strX);
						current += 3;
						break;
					}

					if (StringAt(workingString, current, strTH) || StringAt(workingString, current, strTTH))
					{
						//special case 'thomas', 'thames' or germanic
						if (StringAt(workingString, (current + 2), strOM, strAM)
							|| StringAt(workingString, 0, strVANsp, strVONsp) || StringAt(workingString, 0, strSCH))
						{
							metaphoneData.Add(strT);
						}
						else
						{
							metaphoneData.Add(strO, strT);
						}
						current += 2;
						break;
					}

					if (StringAt(workingString, (current + 1), strT, strD))
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add(strT);
					break;

				case charV:
					if (workingString[current + 1] == charV)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					metaphoneData.Add(strF);
					break;

				case charW:
					//can also be in middle of word
					if (StringAt(workingString, current, strWR))
					{
						metaphoneData.Add(strR);
						current += 2;
						break;
					}

					if ((current == 0) && (IsVowel(workingString[current + 1])
						|| StringAt(workingString, current, strWH)))
					{
						//Wasserman should match Vasserman
						if (IsVowel(workingString[current + 1]))
						{
							metaphoneData.Add(strA, strF);
						}
						else
						{
							//need Uomo to match Womo
							metaphoneData.Add(strA);
						}
					}

					//Arnow should match Arnoff
					if ((current == last && current > 0 && IsVowel(workingString[current - 1]))
						|| StringAt(workingString, (current - 1), strEWSKI, strEWSKY, strOWSKI, strOWSKY)
						|| StringAt(workingString, 0, strSCH))
					{
						metaphoneData.Add(string.Empty, strF);
						current++;
						break;
					}

					//polish e.g. 'filipowicz'
					if (StringAt(workingString, current, strWICZ, strWITZ))
					{
						metaphoneData.Add(strTS, strFX);
						current += 4;
						break;
					}

					//else skip it
					current++;
					break;

				case charX:
					//french e.g. breaux
					if (!((current == last)
						&& (StringAt(workingString, (current - 3), strIAU, strEAU)
						|| StringAt(workingString, (current - 2), strAU, strOU))))
					{
						metaphoneData.Add(strKS);
					}

					if (StringAt(workingString, (current + 1), strC, strX))
					{
						current += 2;
					}
					else
					{
						current++;
					}
					break;

				case charZ:
					//chinese pinyin e.g. 'zhao'
					if (workingString[current + 1] == charH)
					{
						metaphoneData.Add(strJ);
						current += 2;
						break;
					}
					else if (StringAt(workingString, (current + 1), strZO, strZI, strZA)
						|| (isSlavoGermanic && ((current > 0) && workingString[current - 1] != charT)))
					{
						metaphoneData.Add(strS, strTS);
					}
					else
					{
						metaphoneData.Add(strS);
					}

					if (workingString[current + 1] == charZ)
					{
						current += 2;
					}
					else
					{
						current++;
					}
					break;

				default:
					current++;
					break;
			}
		}

		return metaphoneData.ToString();
	}


	static bool IsVowel(this char self)
	{
		return (self == charA) || (self == charE) || (self == charI)
			|| (self == charO) || (self == charU) || (self == charY);
	}

	internal const char charA = 'A';
	internal const char charW = 'W';
	internal const char charK = 'K';
	internal const string strCZ = "CZ";
	internal const string strWITZ = "WITZ";
	internal const string strGN = "GN";
	internal const string strKN = "KN";
	internal const string strPN = "PN";
	internal const string strWR = "WR";
	internal const string strPS = "PS";
	internal const char charX = 'X';
	internal const string strS = "S";
	internal const char charE = 'E';
	internal const char charI = 'I';
	internal const char charO = 'O';
	internal const char charU = 'U';
	internal const char charY = 'Y';
	internal const char charB = 'B';
	internal const char charAdash = 'Ã';
	internal const string strACH = "ACH";
	internal const string strBACHER = "BACHER";
	internal const string strMACHER = "MACHER";
	internal const string strK = "K";
	internal const string strCAESAR = "CAESAR";
	internal const string strCHIA = "CHIA";
	internal const string strCH = "CH";
	internal const string strCHAE = "CHAE";
	internal const string strX = "X";
	internal const string strHARAC = "HARAC";
	internal const string strHARIS = "HARIS";
	internal const string strHOR = "HOR";
	internal const string strHYM = "HYM";
	internal const string strHIA = "HIA";
	internal const string strHEM = "HEM";
	internal const string strCHORE = "CHORE";
	internal const string strVANsp = "VAN ";
	internal const string strVONsp = "VON ";
	internal const string strSCH = "SCH";
	internal const string strORCHES = "ORCHES";
	internal const string strARCHIT = "ARCHIT";
	internal const string strORCHID = "ORCHID";
	internal const string strT = "T";
	internal const string strA = "A";
	internal const string strO = "O";
	internal const string strU = "U";
	internal const string strE = "E";
	internal const string strL = "L";
	internal const string strR = "R";
	internal const string strN = "N";
	internal const string strM = "M";
	internal const string strB = "B";
	internal const string strH = "H";
	internal const string strF = "F";
	internal const string strV = "V";
	internal const string strW = "W";
	internal const string sp = " ";
	internal const string strMC = "MC";
	internal const string strWICZ = "WICZ";
	internal const string strCIA = "CIA";
	internal const string strCC = "CC";
	internal const char charM = 'M';
	internal const string strI = "I";
	internal const string strHU = "HU";
	internal const string strUCCEE = "UCCEE";
	internal const string strUCCES = "UCCES";
	internal const string strKS = "KS";
	internal const string strCK = "CK";
	internal const string strCG = "CG";
	internal const string strCQ = "CQ";
	internal const string strCI = "CI";
	internal const string strCE = "CE";
	internal const string strCY = "CY";
	internal const string strCIO = "CIO";
	internal const string strCIE = "CIE";
	internal const string strspC = " C";
	internal const string strspQ = " Q";
	internal const string strspG = " G";
	internal const string strC = "C";
	internal const string strQ = "Q";
	internal const char charC = 'C';
	internal const char charD = 'D';
	internal const string strDG = "DG";
	internal const string strY = "Y";
	internal const string strJ = "J";
	internal const string strTK = "TK";
	internal const string strDT = "DT";
	internal const string strDD = "DD";
	internal const char charF = 'F';
	internal const char charG = 'G';
	internal const char charH = 'H';
	internal const string strD = "D";
	internal const string strG = "G";
	internal const char charN = 'N';
	internal const string strEY = "EY";
	internal const string strLI = "LI";
	internal const string strKL = "KL";
	internal const string strES = "ES";
	internal const string strEP = "EP";
	internal const string strEB = "EB";
	internal const string strEL = "EL";
	internal const string strIB = "IB";
	internal const string strIL = "IL";
	internal const string strIN = "IN";
	internal const string strIE = "IE";
	internal const string strEI = "EI";
	internal const string strER = "ER";
	internal const string strDANGER = "DANGER";
	internal const string strRANGER = "RANGER";
	internal const string strMANGER = "MANGER";
	internal const string strRGY = "RGY";
	internal const string strOGY = "OGY";
	internal const string strAGGI = "AGGI";
	internal const string strOGGI = "OGGI";
	internal const string strIERsp = "IER ";
	internal const string strJOSE = "JOSE";
	internal const string strSANsp = "SAN ";
	internal const string strZ = "Z";
	internal const char charJ = 'J';
	internal const char charL = 'L';
	internal const string strILLO = "ILLO";
	internal const string strILLA = "ILLA";
	internal const string strALLE = "ALLE";
	internal const string strAS = "AS";
	internal const string strOS = "OS";
	internal const string strUMB = "UMB";
	internal const char charOdash = 'Ð';
	internal const char charP = 'P';
	internal const string strP = "P";
	internal const char charQ = 'Q';
	internal const string strME = "ME";
	internal const string strMA = "MA";
	internal const char charR = 'R';
	internal const char charS = 'S';
	internal const string strISL = "ISL";
	internal const string strYSL = "YSL";
	internal const string strSUGAR = "SUGAR";
	internal const string strSH = "SH";
	internal const string strHEIM = "HEIM";
	internal const string strHOEK = "HOEK";
	internal const string strHOLM = "HOLM";
	internal const string strHOLZ = "HOLZ";
	internal const string strSIO = "SIO";
	internal const string strSIA = "SIA";
	internal const string strSIAN = "SIAN";
	internal const string strSC = "SC";
	internal const string strOO = "OO";
	internal const string strEN = "EN";
	internal const string strUY = "UY";
	internal const string strED = "ED";
	internal const string strEM = "EM";
	internal const string strSK = "SK";
	internal const string strAI = "AI";
	internal const string strOI = "OI";
	internal const string strTION = "TION";
	internal const string strTIA = "TIA";
	internal const string strTCH = "TCH";
	internal const char charT = 'T';
	internal const string strTH = "TH";
	internal const string strTTH = "TTH";
	internal const string strOM = "OM";
	internal const string strAM = "AM";
	internal const char charV = 'V';
	internal const string strWH = "WH";
	internal const string strEWSKI = "EWSKI";
	internal const string strEWSKY = "EWSKY";
	internal const string strOWSKI = "OWSKI";
	internal const string strOWSKY = "OWSKY";
	internal const string strFX = "FX";
	internal const string strTS = "TS";
	internal const string strEAU = "EAU";
	internal const string strIAU = "IAU";
	internal const string strAU = "AU";
	internal const string strOU = "OU";
	internal const char charZ = 'Z';
	internal const string strZA = "ZA";
	internal const string strZI = "ZI";
	internal const string strZO = "ZO";
	internal const string strET = "ET";


	static bool StartsWith(this string self, StringComparison comparison, params string[] strings)
	{
		foreach (var str in strings)
		{
			if (self.StartsWith(str, comparison))
			{
				return true;
			}
		}
		return false;
	}

	static bool StringAt(this string self, int startIndex, params string[] strings)
	{
		if (startIndex < 0)
		{
			startIndex = 0;
		}

		foreach (var str in strings)
		{
			if (self.IndexOf(str, startIndex, StringComparison.OrdinalIgnoreCase) >= startIndex)
			{
				return true;
			}
		}
		return false;
	}


	internal class MetaphoneData
	{
		readonly StringBuilder _primary = new(5);
		readonly StringBuilder _secondary = new(5);


		#region Properties

		internal bool Alternative { get; set; }
		internal int PrimaryLength => _primary.Length;

		internal int SecondaryLength => _secondary.Length;

		#endregion


		internal void Add(string main)
		{
			if (main != null)
			{
				_primary.Append(main);
				_secondary.Append(main);
			}
		}

		internal void Add(string main, string alternative)
		{
			if (main != null)
			{
				_primary.Append(main);
			}

			if (alternative != null)
			{
				Alternative = true;
				if (alternative.Trim().Length > 0)
				{
					_secondary.Append(alternative);
				}
			}
			else if (main != null && main.Trim().Length > 0)
			{
				_secondary.Append(main);
			}
		}

		internal (string Primary, string Secondary) GetCodes()
		{
			var prim = _primary.ToString();
			var sec = _secondary.ToString();
			if (prim.Length > 4)
			{
				prim = prim[..4];
			}

			if (sec.Length > 4)
			{
				sec = sec[..4];
			}

			return (prim, sec);
		}

		public override string ToString()
		{
			var ret = (Alternative ? _secondary : _primary).ToString();

			if (ret.Length > 4)
			{
				ret = ret[..4];
			}

			return ret;
		}
	}
}