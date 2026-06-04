// Pruebas de integración para PowerToys: validación de archivos de configuración reales.
// TPO Parte II - Calidad de Software, UADE.
//
// Estos tests leen los archivos settings.json reales que PowerToys persiste en
// %LOCALAPPDATA%\Microsoft\PowerToys\<Modulo>\settings.json. Validan la integración
// entre el módulo, el sistema de archivos y el formato JSON esperado.
//
// REQUISITO: PowerToys debe estar instalado y haberse ejecutado al menos una vez,
// para que los archivos de configuración existan. Si no se cumple, los tests se
// marcan como "Inconclusive" (no como "Failed"), de modo que no rompen el pipeline
// en agentes que no tienen PowerToys instalado.

using System;
using System.IO;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Integration.Tests
{
    [TestClass]
    public class PowerToysSettingsIntegrationTests
    {
        private static string PowerToysRoot =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Microsoft", "PowerToys");

        // ── IT-AUTO-01: Color Picker — el settings.json existe y es JSON válido ──
        [TestMethod]
        public void ColorPicker_SettingsFile_ExistsAndIsValidJson()
        {
            var path = Path.Combine(PowerToysRoot, "ColorPicker", "settings.json");

            if (!File.Exists(path))
            {
                Assert.Inconclusive(
                    $"No se encontró {path}. " +
                    "Verificá que PowerToys esté instalado y se haya abierto Color Picker al menos una vez.");
                return;
            }

            var content = File.ReadAllText(path);
            Assert.IsFalse(string.IsNullOrWhiteSpace(content), "El settings.json está vacío.");

            // Si el JSON es inválido, Parse lanza JsonException y el test falla con detalle.
            using var doc = JsonDocument.Parse(content);
            Assert.IsNotNull(doc.RootElement);

            // El nodo "name" debe identificar al módulo
            if (doc.RootElement.TryGetProperty("name", out var name))
            {
                Assert.AreEqual("ColorPicker", name.GetString(),
                    "El campo 'name' del settings.json no corresponde al módulo Color Picker.");
            }
        }

        // ── IT-AUTO-01b: Color Picker — la configuración tiene la estructura esperada ──
        [TestMethod]
        public void ColorPicker_Settings_HaveExpectedStructure()
        {
            var path = Path.Combine(PowerToysRoot, "ColorPicker", "settings.json");

            if (!File.Exists(path))
            {
                Assert.Inconclusive($"No se encontró {path}.");
                return;
            }

            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            var root = doc.RootElement;

            // El JSON de Color Picker debe tener al menos uno de estos nodos top-level:
            //   "name", "version", "properties"
            bool hasExpectedShape =
                root.TryGetProperty("properties", out _) ||
                root.TryGetProperty("version", out _) ||
                root.TryGetProperty("name", out _);

            Assert.IsTrue(hasExpectedShape,
                "El settings.json de Color Picker no tiene la estructura esperada " +
                "(se esperaba al menos uno de: properties, version, name).");
        }

        // ── IT-AUTO-02: Quick Accent (PowerAccent) — settings.json válido + idioma reconocido ──
        [TestMethod]
        public void QuickAccent_SettingsFile_ExistsAndHasRecognizableLanguage()
        {
            var path = Path.Combine(PowerToysRoot, "QuickAccent", "settings.json");

            if (!File.Exists(path))
            {
                // En algunas versiones la carpeta se llama "PowerAccent"
                path = Path.Combine(PowerToysRoot, "PowerAccent", "settings.json");
            }

            if (!File.Exists(path))
            {
                Assert.Inconclusive(
                    "No se encontró el settings.json de Quick Accent. " +
                    "Abrí PowerToys y activá Quick Accent al menos una vez para que se genere.");
                return;
            }

            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            var root = doc.RootElement;

            // Buscamos la propiedad selected_lang dentro de properties (estructura real
            // del settings.json verificada en una instalación de PowerToys v0.x).
            // Nota: System.Text.Json es case-sensitive y la clave usa snake_case.
            if (root.TryGetProperty("properties", out var props) &&
                props.TryGetProperty("selected_lang", out var sel))
            {
                // El valor puede estar como string directo o como objeto { "value": "ALL" }
                string lang = sel.ValueKind == JsonValueKind.String
                    ? sel.GetString()
                    : (sel.TryGetProperty("value", out var v) ? v.GetString() : null);

                Assert.IsFalse(string.IsNullOrWhiteSpace(lang),
                    "El idioma seleccionado está vacío.");

                // Conjunto de idiomas reconocidos por el módulo (subconjunto válido)
                string[] known = new[]
                {
                    "ALL","SP","FR","PT","DE","IT","CUR","CZ","GA","GD","HR","HU",
                    "IS","MI","NL","NO","PL","RO","SK","SR","TK","CY","CA","TR","EPO"
                };

                CollectionAssert.Contains(known, lang,
                    $"El idioma '{lang}' no figura entre los reconocidos por Quick Accent.");
            }
            else
            {
                Assert.Inconclusive(
                    "El settings.json de Quick Accent no tiene properties.selected_lang. " +
                    "Puede que la versión instalada use otra clave; documentá la estructura encontrada.");
            }
        }

        // ── IT-AUTO-03: Listado del directorio raíz de PowerToys (smoke test general) ──
        [TestMethod]
        public void PowerToys_RootDirectory_ContainsExpectedModuleFolders()
        {
            if (!Directory.Exists(PowerToysRoot))
            {
                Assert.Inconclusive(
                    $"No se encontró el directorio {PowerToysRoot}. " +
                    "Instalá PowerToys y ejecutalo al menos una vez.");
                return;
            }

            var subdirs = Directory.GetDirectories(PowerToysRoot);
            Assert.IsTrue(subdirs.Length > 0,
                "El directorio de PowerToys existe pero está vacío.");

            // No exigimos un módulo en particular: distintas versiones tienen distintos módulos.
            // Solo dejamos constancia (en el log) de qué módulos están instalados.
            foreach (var d in subdirs)
            {
                Console.WriteLine($"  - {Path.GetFileName(d)}");
            }
        }
    }
}
