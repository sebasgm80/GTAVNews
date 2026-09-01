# GTA V News - Xbox One Dev Mode

App de noticias GTA V para Xbox One. Mantiene el modo dev activo.

## Paso 1: Sube a GitHub

```bash
cd GTAVNews
git init
git add .
git commit -m "Initial"
git remote add origin https://github.com/TU_USUARIO/GTAVNews.git
git push -u origin main
```

GitHub Actions construira el MSIX automaticamente cada 2 meses.

## Paso 2: Descarga el MSIX

Ve a: Actions -> "Build GTA V News" -> descarga el artifacto "GTAVNews-MSIX"

## Paso 3: Despliega en la Xbox

```bash
./deploy.sh GTAVNews.msix
```

O abre Safari en http://192.168.1.129:11443 y sube el .msix manualmente.

## Credenciales

Edita deploy.sh con tu usuario y contraseña del Device Portal de la Xbox.

## RSS de noticias

Cambialo en MainPage.xaml.cs (linea 19) si quieres otra fuente.