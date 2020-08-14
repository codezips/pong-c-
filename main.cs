/*
Author: Raza Tahir
Email: Razat51@csu.fullerton.edu
Course: CPSC 223n
Assignment #6
Due Date: December 18th, 2016 at 2 am
Purpose: Pong Game
Purpose of this file: Launch the window using the graphical area
*/
 
using System;
using System.Windows.Forms; 

public class Paddle {

	public static void Main() {
  
		System.Console.WriteLine("Welcome to the paddle demonstration program.");
      		Paddleframe paddleapplication = new Paddleframe();
      		Application.Run(paddleapplication);
      		System.Console.WriteLine("This program has ended. Goodbye!");

   	}//End of Main method

}//End of Paddle class
