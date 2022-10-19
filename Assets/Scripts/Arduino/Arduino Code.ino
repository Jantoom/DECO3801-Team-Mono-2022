//int ledPin = 13;
int Pin1 = 2;
int Pin2 = 3;
int Pin3 = 4;
int Pin4 = 5;
int Pin5 = 6;
int Pin6 = 7;
//int val = 0;
bool Down1 = false;
bool Down2 = false;
bool Down3 = false;
bool Down4 = false;
bool Down5 = false;
bool Down6 = false;

void setup() {
  Serial.begin(9600);
  // put your setup code here, to run once:
  //pinMode(ledPin, OUTPUT);
  pinMode(Pin1, INPUT);
  pinMode(Pin2, INPUT);
  pinMode(Pin3, INPUT);
  pinMode(Pin4, INPUT);
  pinMode(Pin5, INPUT);
  pinMode(Pin6, INPUT);

  digitalWrite(Pin1, HIGH);
  digitalWrite(Pin2, HIGH);
  digitalWrite(Pin3, HIGH);
  digitalWrite(Pin4, HIGH);
  digitalWrite(Pin5, HIGH);
  digitalWrite(Pin6, HIGH);
}

void loop() {
  if (digitalRead(Pin1) == HIGH) {
    if (Down1 == false) {
      Down1 = true;
      Send(1);
    }
  } else {
    Down1 = false;
  }
  
  if (digitalRead(Pin2) == HIGH) {
    if (Down2 == false) {
      Down2 = true;
      Send(2);
    }
  } else {
    Down2 = false;
  }

  if (digitalRead(Pin3) == HIGH) {
    if (Down3 == false) {
      Down3 = true;
      Send(3);
    }
  } else {
    Down3 = false;
  }

  if (digitalRead(Pin4) == HIGH) {
    if (Down4 == false) {
      Down4 = true;
      Send(4);
    }
  } else {
    Down4 = false;
  }

  if (digitalRead(Pin5) == HIGH) {
    if (Down5 == false) {
      Down5 = true;
      Send(5);
    }
  } else {
    Down5 = false;
  }

  if (digitalRead(Pin6) == HIGH) {
    if (Down6 == false) {
      Down6 = true;
      Send(6);
    }
  } else {
    Down6 = false;
  }

  delay(30);
}

void Send(int signal) {
  // Send twice to guarantee intended recipient gets the signal, 
  // and is not consumed by the unintended recipient
  Serial.write(signal);
  Serial.write(signal);
  // Ensure writes are flushed together
  Serial.flush();
}
