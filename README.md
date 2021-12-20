# Pixel
Confirmación de Lectura para Correos Electrónicos
## ¿Qué es esto?
Utilidad sencilla para poder incorporar a los correos electrónicos un "pixel", que es el que a su vez nos dará la confirmación de lectura del mensaje enviado.

Esta utilidad se comunicará con un servidor, que es el que estará "escuchando" y se encargará de procesar la información.

Tiene un defecto, y es que el pixel no funcionará con ninguna dirección de correo alojada en los servidores de google (@gmail.com). Mas información [aquí](https://blog.filippo.io/how-the-new-gmail-image-proxy-works-and-what-this-means-for-you/)
## Capturas de pantalla
![Menú Principal](https://i.imgur.com/roY3vFk.png)

![Configurador](https://i.imgur.com/Ar4ZHLk.png)

![Crear Pixel](https://i.imgur.com/4tn0KuB.png)

![Ver Pixel Usado](https://i.imgur.com/rntfnwc.png)

![Ver Pixel Sin Usar](https://i.imgur.com/JU1qpY9.png)

## Requerimientos
Para poder hostear (y usar) una instancia de este proyecto, es necesario disponer de:
> Un servidor web con php

> Una base de datos MySQL

> Un ordenador Windows

> Un cliente de correo que permita la redacción en html
## Instalación
Para empezar, [descarga](https://github.com/nicoagr/pixel/releases/latest/download/pixel.exe) el programa, y ejecútalo. El ejecutable no está firmado, así que es posible que aparezca un aviso de Windows SmartScreen advirtiéndonos sobre los posibles riesgos.

Nos aparecerá la pantalla del configurador, en el que tendremos que introducir servidor, nombre de usuario, contraseña y tabla de una base de datos MySQL que tengamos alojada en internet. Si no están las tablas creadas, el configurador las creará automáticamente. Después, saldrá un botón con el que podremos descargar los archivos que tendremos que subir a nuestro servidor web. Una vez subidos, asegúrate que estos archivos son visibles, e introduce la ruta completa del archivo firmacorreo.php.

Si todo ha ido bien, el programa ya estaría configurado y listo para funcionar. Si aparece algún error o algo inesperado que no puedas solucionar, no dudes en crear un issue en esta página de proyecto de github.
## Uso habitual
<!> Aviso <!> Las confirmaciones de lectura enviadas a un correo electrónico alojado en los servidores de google resultarán en información errónea.
La configuración dependerá de el cliente de correo usado, pero en la mayoría de los casos, las instrucciones son las siguientes:
> Generar un pixel, y copiar el código HTML generado.
> Pegarlo abajo del todo en el e-mail que queramos enviar.
> Podremos consultar el estado de nuestro e-mail en la pestaña "Ver Pixel", y ver si se ha abierto o no. También existe la posibilidad de eliminar un registro si éste no ha sido abierto 
## Descarga
[[Windows 7, 10, 11]](https://github.com/nicoagr/pixel/releases/latest/download/pixel.exe)
### Legal
*Este proyecto NO tiene una licencia "de código abierto". Para obtener más información sobre licencias de código abierto, haz click [aquí](https://opensource.org/faq). Si quieres saber qué significa que este proyecto no tenga una licencia "de código abierto", haz click [aquí](https://choosealicense.com/no-permission/)*
