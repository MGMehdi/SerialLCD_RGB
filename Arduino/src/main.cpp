#include <Arduino.h>

// include the library code:
#include <LiquidCrystal.h>

// initialize the library by associating any needed LCD interface pin
// with the arduino pin number it is connected to
const int rs = 2, en = 3, d4 = 4, d5 = 5, d6 = 6, d7 = 7;
const int red = 8, green = 9, blue = 10;
LiquidCrystal lcd(rs, en, d4, d5, d6, d7);
char chara = ' ';
String message = "", redValue = "", greenValue = "", blueValue = "";

bool state = false;

/*********************************************************************************************************/
void setup()
{
  // set up the LCD's number of columns and rows:
  lcd.begin(16, 2);
  lcd.clear();

  // initialize the serial communications:
  Serial.begin(9600);
}

/*********************************************************************************************************/
void loop()
{
  /* Reception charactere et enregistrement du mot */
  if (Serial.available())
  {
    delay(100);
    //Stocker le messagee recu
    while (Serial.available() > 0)
    {
      chara = Serial.read();
      if ((int)chara != 10 && (int)chara != 13) // Ignorer LF CR
      {
        message += chara;
      }
    }
  }

  /* Verifie le permier charactere du message */
  switch ((int)message.charAt(0))
  {
  /* # --> ecrire le message sur le LCD */
  case 35:
    lcd.clear();
    for (int i = 1; i < (int)message.length(); i++)
    {
      lcd.print(message.charAt(i));
    }
    break;

  /* * --> changer la couleur de la LED*/
  case 42:
    switch (message.charAt(1))
    {
    /* Red */
    case 'R':
      for (int i = 2; i < message.length(); i++)
      {
        redValue += message.charAt(i);
      }
      analogWrite(red, redValue.toInt());
      break;

    /* Green */
    case 'G':
      for (int i = 2; i < message.length(); i++)
      {
        greenValue += message.charAt(i);
      }
      analogWrite(green, greenValue.toInt());
      break;

    /* Blue */
    case 'B':
      for (int i = 2; i < message.length(); i++)
      {
        blueValue += message.charAt(i);
      }
      analogWrite(blue, blueValue.toInt());
      break;
    }
    break;

  /* + --> eteindre la LED */
  case 43:
    redValue = "0";
    greenValue = "0";
    blueValue = "0";
    analogWrite(red, redValue.toInt());
    analogWrite(green, greenValue.toInt());
    analogWrite(blue, blueValue.toInt());
    break;
  }

  /* Vider le  message pour recevoir le suivant */
  redValue = "";
  greenValue = "";
  blueValue = "";
  message = "";
}
