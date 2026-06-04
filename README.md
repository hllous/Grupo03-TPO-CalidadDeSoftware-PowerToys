# 🧪 Plan de Pruebas — Microsoft PowerToys

> **Materia:** Calidad de Software  
> **Herramienta bajo prueba:** [Microsoft PowerToys](https://github.com/microsoft/PowerToys)  
> **Tipo de proyecto:** Plan de pruebas con tests unitarios, de integración y pipeline CI/CD

---

## 📋 Descripción

Este repositorio contiene el plan de pruebas para un conjunto de herramientas de **Microsoft PowerToys**, desarrollado en el marco de la materia **Calidad de Software**. El objetivo es validar el correcto funcionamiento de las utilidades seleccionadas mediante distintos niveles de prueba, asegurando calidad, confiabilidad y comportamiento esperado bajo diferentes condiciones.

Las herramientas evaluadas son:

| Herramienta       | Descripción breve                                                                 |
|-------------------|-----------------------------------------------------------------------------------|
| **Quick Accent**  | Permite ingresar caracteres con acento manteniendo presionada una tecla base      |
| **Color Picker**  | Captura el color de cualquier píxel en pantalla y lo convierte a distintos formatos |
| **Text Extractor**| Extrae texto de imágenes o áreas de la pantalla mediante OCR                      |
| **Always On Top** | Permite anclar cualquier ventana para que permanezca siempre visible              |

---


---

## ⚙️ Pipeline CI/CD — GitHub Actions

El proyecto incluye un workflow de integración y entrega continua configurado en `.github/workflows/`.

### `ci.yml` — Integración Continua

Se ejecuta en cada **push** y **pull request** hacia las ramas `main` y `develop`.

## 👥 Integrantes

* Nicolas Facundo Llousas ; LU: 1147795 ; Ingeniería en Informática
* Lucas Valentin Vazquez ; LU: 1148671 ; Ingeniería en Informática
* Nicolas Casais ; LU: 1185300 ; Ingeniería en Informática
* Ramiro Landajo ; LU: 1155576 ; Ingeniería en Informática
* Santiago Larre ; LU: 1158242 ; Ingeniería en Informática



---

<div align="center">
  <sub>Facultad de Ingeniería — Calidad de Software</sub>
</div>
