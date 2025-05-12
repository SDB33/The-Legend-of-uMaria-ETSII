/*
editor:

Debería haber dos estilos de cámara: El original y uno sin restricciones de cuadrícula

si dejas pulsado el botón de borrar durante “x” segundos, se borra todo el nivel

Caparlo a 60 fps


que la ventana de guardar/cargar archivo tenga un nombre gracioso

Hay que poner el código bien, comentado, de forma más general, con todas las cositas estas de unity como tooltip y header y eso

cada bloque o ciertos bloques deberían tener un límite de cuántas veces puedes ponerlo. Ya sea que en cada momento hay un texto con un contador y va bajando o simplemente no deje al final ponerlo



Sorting Layer

fons: va a actuar como fondo y va a ser para el lienzo
Default: para el suelo y paredes
Entitats: enemigos o personaje principal
Ui: para la ui

Layer

default: suelos y paredes
water = entidades y objetos



Los que tengan que interactuar con el ambiente serán collider
Los que no serán trigger

Esto es una desventaja puesto que si es collider no va a interactuar ni con las paredes ni con los enemigos de su tipo y si es trigger al revés y a lo mejor queremos que
pueda chocarse contra las paredes pero no contra los enemigos de su tipo


El hecho de poner el suelo como trigger va a hacer que siempre estén colisionando con él lo cual es cutre, habría que hacerlos tilemap desde un principio y ver como se puede poner colliders solo a las paredes




*/