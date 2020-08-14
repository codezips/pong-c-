/*
Author: Raza Tahir
Email: Razat51@csu.fullerton.edu
Course: CPSC 223n
Assignment #6
Due Date: December 18th, 2016 at 2 am
Purpose: Pong Game
Purpose of this file: Create a random direction for the ball NOT between 80 && 90 degrees, and 260 && 280 degrees.
*/

using System;
using System.Windows.Forms;
using static System.Math;

namespace Buttontypes {

	public class NoFocusButton : Button { 
  
		public NoFocusButton () {

			SetStyle(ControlStyles.Selectable,false);

		}

	}   //End of NoFocusButton class

}//End of Buttontypes namespace

public class algorithmlogic {   

	private System.Random random = new System.Random();
	private double zero_or_one;
	private System.Random randomgenerator = new System.Random();
	private double randomnumber;

	public double randomize() { // return a random number, 0 or 1! This is so I know if the ball should go right or left!
		random = new System.Random();		
		zero_or_one = random.NextDouble();
		zero_or_one = Round ( zero_or_one );
		return zero_or_one;
	}

	public double get_random_direction() {
		randomgenerator = new System.Random();
		randomnumber = randomgenerator.NextDouble();
		if ( randomize() == 1 ) randomnumber = randomnumber - 0.5; // if number is 1, then angle is between 90 & 270 degrees
		else randomnumber = randomnumber + 0.5; // else if number is 0, then angle is between -90 & +90 degrees
		double ball_angle_radians = System.Math.PI * randomnumber;
		return ball_angle_radians;

        }//End of method get_random_direction

}//End of algorithmlogic class
