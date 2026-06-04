// Copyright (c) Microsoft Corporation. Licensed under the MIT license.
// Pruebas unitarias del módulo Quick Accent (cobertura de caracteres por idioma).
// Framework: MSTest (estándar del proyecto PowerToys).

using PowerAccent.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PowerAccent.Tests
{
    [TestClass]
    public class LanguagesTests
    {
        // ── UT-04: Cobertura de acentos del español ──
        [TestMethod]
        public void Spanish_LetterN_ContainsEnye()
        {
            var variants = Languages.GetDefaultLetterKey(
                LetterKey.VK_N, new[] { Language.SP });
            CollectionAssert.Contains(variants, "ñ");
        }

        [TestMethod]
        public void Spanish_LetterU_ContainsAccentAndDiaeresis()
        {
            var variants = Languages.GetDefaultLetterKey(
                LetterKey.VK_U, new[] { Language.SP });
            CollectionAssert.Contains(variants, "ú");
            CollectionAssert.Contains(variants, "ü");
        }

        [DataTestMethod]
        [DataRow(LetterKey.VK_A, "á")]
        [DataRow(LetterKey.VK_E, "é")]
        [DataRow(LetterKey.VK_I, "í")]
        [DataRow(LetterKey.VK_O, "ó")]
        public void Spanish_VowelsContainExpectedAccent(LetterKey key, string expected)
        {
            var variants = Languages.GetDefaultLetterKey(key, new[] { Language.SP });
            CollectionAssert.Contains(variants, expected);
        }

        // ── French ofrece más variantes para 'a' que el español ──
        [TestMethod]
        public void French_LetterA_HasMoreVariantsThanSpanish()
        {
            var french = Languages.GetDefaultLetterKey(LetterKey.VK_A, new[] { Language.FR });
            var spanish = Languages.GetDefaultLetterKey(LetterKey.VK_A, new[] { Language.SP });
            Assert.IsTrue(french.Length > spanish.Length,
                "El francés debe ofrecer más acentos para 'a' (à, â, ä, ã, æ).");
        }

        // ── UT-05: Idioma sin variantes para una tecla no produce opciones espurias ──
        [TestMethod]
        public void German_LetterI_ReturnsNoVariants()
        {
            var variants = Languages.GetDefaultLetterKey(
                LetterKey.VK_I, new[] { Language.DE });
            Assert.AreEqual(0, variants.Length,
                "El alemán no define variantes para 'i'.");
        }

        // ── Combinación de idiomas: las variantes se unen sin duplicados ──
        [TestMethod]
        public void MultipleLanguages_MergeWithoutDuplicates()
        {
            // Español y portugués comparten "á" para la tecla A.
            var variants = Languages.GetDefaultLetterKey(
                LetterKey.VK_A, new[] { Language.SP, Language.PT });

            int countA = 0;
            foreach (var v in variants)
            {
                if (v == "á")
                {
                    countA++;
                }
            }

            Assert.AreEqual(1, countA, "El carácter 'á' no debe duplicarse al combinar idiomas.");
        }

        // ── Robustez: lista de idiomas vacía o nula devuelve array vacío ──
        [TestMethod]
        public void EmptyLanguageList_ReturnsEmptyArray()
        {
            var variants = Languages.GetDefaultLetterKey(LetterKey.VK_A, new Language[0]);
            Assert.AreEqual(0, variants.Length);
        }

        [TestMethod]
        public void NullLanguageList_ReturnsEmptyArray()
        {
            var variants = Languages.GetDefaultLetterKey(LetterKey.VK_A, null);
            Assert.AreEqual(0, variants.Length);
        }
    }
}
