# -*- coding: utf-8 -*-
import string
import sys
import os
import socket

def printMainMenu():
  os.system("clear")
  print("1. Get Name")
  print("2. Get Street")
  print("3. Get House Number")
  print("4. Get Zip")
  print("5. Get City")
  print("6. Get State")
  print("7. Get Social Security Number")
  print("8. Add Person")
  print("9. Exit")
  return

def addPerson():
  os.system("clear")
  firstName = raw_input("Enter first name: ")
  lastName = raw_input("Enter last name: ")
  street = raw_input("Enter street: ") 
  houseNo = raw_input("Enter house number: ") 
  zipCode = raw_input("Enter zip code: ")
  city = raw_input("Enter city: ")
  state = raw_input("Enter state: ") 
  ssn = raw_input("Enter ssn (no dashes): ")

  message = "ADD_PERSON:"
  message = message + firstName + ":" + lastName + ":"
  message = message + ssn + ":"
  message = message + street + ":"
  message = message + houseNo + ":"
  message = message + city + ":"
  message = message + state + ":"
  message = message + zipCode
  #print("message for server:\n" + message)
  return message

if (len(sys.argv) != 3):
  print("Incorrect number of arguments! Please provide port number and address only.")
  sys.exit(0)

portNo = (int)(sys.argv[1])
addr = sys.argv[2]
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect((addr, portNo))

while(1):
  printMainMenu()
  option = raw_input("\nPlease enter an option: ")

  if(option == "9"):
    sock.send("EXIT")
    sys.exit(0)
  if(option != "8" and option != "7"):
    ssn = input("Enter ssn (no dashses): ")
  elif(option == "8"):
    sock.send(addPerson())
    data = sock.recv(1024)
    if(data != "1"):
      print("Error adding: " + data.split(":")[1])
      temp = raw_input();
    continue
  elif(option == "7"):
    lastName = input("Enter last name: ")
    sock.send("GET_SSNS:" + str(lastName))
    data = sock.recv(1024)
    if(data[0] == "0"):
      print("Error: " + data.split(":")[1])
      continue
    print("SSNs for "+ str(lastName) + ":")
    for i in range(len(data.split(":"))):
      if(i != 0):
	print(data.split(":")[i])
    temp = raw_input("Hit enter to continue");
    continue

  if(option == "1"):
    message = "GET_NAME:" + str(ssn)
    sock.send(message)
    data = sock.recv(1024)
    print(data.split(":")[1])
    temp = raw_input("Hit enter to continue")
  elif(option == "2"):
    message = "GET_STREET:" + str(ssn)
    sock.send(message)
    data = sock.recv(1024)
    print(data.split(":")[1])
    temp = raw_input("Hit enter to continue")
  elif(option == "3"):
    message = "GET_HOUSENO:" + str(ssn)
    sock.send(message)
    data = sock.recv(1024)
    print(data.split(":")[1])
    temp = raw_input("Hit enter to continue")
  elif(option == "4"):
    message = "GET_ZIP:" + str(ssn)
    sock.send(message)
    data = sock.recv(1024)
    print(data.split(":")[1])
    temp = raw_input("Hit enter to continue")
  elif(option == "5"):
    message = "GET_CITY:" + str(ssn)
    sock.send(message)
    data = sock.recv(1024)
    print(data.split(":")[1])
    temp = raw_input("Hit enter to continue")
  elif(option == "6"):
    message = "GET_STATE:" + str(ssn)
    sock.send(message)
    data = sock.recv(1024)
    print(data.split(":")[1])
    temp = raw_input("Hit enter to continue")