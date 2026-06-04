// Copyright (c) Microsoft Corporation. Licensed under the MIT license.
// Pruebas unitarias del módulo Color Picker (conversión de color).
// Framework: MSTest (estándar del proyecto PowerToys).

using System;
using ColorPicker.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ColorPicker.Tests
{
    [TestClass]
    public class ColorConverterTests
    {
        // ── UT-01: Conversión RGB → HEX de colores conocidos ──
        [TestMethod]
        public void RgbToHex_PureRed_ReturnsFF0000()
        {
            var hex = ColorConverter.RgbToHex(255, 0, 0);
            Assert.AreEqual("#FF0000", hex);
        }

        [DataTestMethod]
        [DataRow(255, 255, 255, "#FFFFFF")]
        [DataRow(0, 0, 0, "#000000")]
        [DataRow(0, 255, 0, "#00FF00")]
        [DataRow(0, 0, 255, "#0000FF")]
        [DataRow(18, 52, 86, "#123456")]
        public void RgbToHex_KnownValues_AreCorrect(int r, int g, int b, string expected)
        {
            Assert.AreEqual(expected, ColorConverter.RgbToHex(r, g, b));
        }

        // ── UT-02: Conversión reversible HEX → RGB → HEX (round-trip) ──
        [DataTestMethod]
        [DataRow("#FF0000")]
        [DataRow("#00FF00")]
        [DataRow("#123456")]
        [DataRow("#ABCDEF")]
        public void HexToRgb_RoundTrip_PreservesValue(string original)
        {
            var (r, g, b) = ColorConverter.HexToRgb(original);
            var roundTrip = ColorConverter.RgbToHex(r, g, b);
            Assert.AreEqual(original.ToUpperInvariant(), roundTrip);
        }

        [TestMethod]
        public void HexToRgb_WithoutHashPrefix_IsAccepted()
        {
            var (r, g, b) = ColorConverter.HexToRgb("123456");
            Assert.AreEqual(18, r);
            Assert.AreEqual(52, g);
            Assert.AreEqual(86, b);
        }

        // ── UT-03: Conversión RGB → HSL de valores límite ──
        [TestMethod]
        public void RgbToHsl_White_ReturnsLightnessOne()
        {
            var (h, s, l) = ColorConverter.RgbToHsl(255, 255, 255);
            Assert.AreEqual(0.0, s, 0.001, "El blanco no tiene saturación.");
            Assert.AreEqual(1.0, l, 0.001, "El blanco tiene luminosidad máxima.");
        }

        [TestMethod]
        public void RgbToHsl_Black_ReturnsLightnessZero()
        {
            var (h, s, l) = ColorConverter.RgbToHsl(0, 0, 0);
            Assert.AreEqual(0.0, s, 0.001);
            Assert.AreEqual(0.0, l, 0.001);
        }

        [TestMethod]
        public void RgbToHsl_PureRed_ReturnsHueZero()
        {
            var (h, s, l) = ColorConverter.RgbToHsl(255, 0, 0);
            Assert.AreEqual(0.0, h, 0.001);
            Assert.AreEqual(1.0, s, 0.001);
            Assert.AreEqual(0.5, l, 0.001);
        }

        [TestMethod]
        public void RgbToHsl_PureGreen_ReturnsHue120()
        {
            var (h, _, _) = ColorConverter.RgbToHsl(0, 255, 0);
            Assert.AreEqual(120.0, h, 0.001);
        }

        // ── Pruebas de robustez (manejo de errores) ──
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void HexToRgb_InvalidLength_ThrowsFormatException()
        {
            ColorConverter.HexToRgb("#12345");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HexToRgb_Empty_ThrowsArgumentException()
        {
            ColorConverter.HexToRgb("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RgbToHex_OutOfRange_ThrowsException()
        {
            ColorConverter.RgbToHex(256, 0, 0);
        }
    }
}
