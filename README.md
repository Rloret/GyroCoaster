# GyroCoaster

Endless runner en 3D, basado en el uso de un arduino con un acelerómetro y un giroscopio. 

El jugador deberá conseguir el mayor número de monedas posibles para poder completar el nivel.
Para ello, empleará el acelerómetro para modificar la pendiente del trayecto girándolo hacia arriba y hacia abajo.
Cada moneda recogida, prolonga el tiempo de juego.

Si el tiempo llega a su fin antes de que el jugador complete el nivel,
fracasará su misión. Además, durante el transcurso del juego,
aparecerán diferentes tumbas para obstaculizarle el paso. Si choca con alguna de ellas también perderá.
Para poder evitarlas, el jugador deberá cortar la malla, girando hacia un lado el dispositivo arduino.
En ese momento, se detendrá la continuación de la malla camino y el jugador caerá sobre otra malla que se creará por debajo.
No obstante, el jugador no puede abusar de este mecanismo, ya que está restringido cada cierto tiempo y,
además, si se encuentra en la zona más baja de la pantalla tampoco se le permitirá hacerlo.
Para mantenerle informado del momento en que no puede cortar la malla debido a la altura, el camino se tornará de un rojizo más intenso.
