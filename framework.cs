/*
Author: Raza Tahir
Email: Razat51@csu.fullerton.edu
Course: CPSC 223n
Assignment #6
Due Date: December 18th, 2016 at 2 am
Purpose: Pong Game
Purpose of this file: Class declaration and key inputs
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Timers;
using Buttontypes;
using System.Text;
using System.Linq;
using static System.Math;

public class Paddleframe : Form {

	//Declare constants 
	private const int formwidth = 1280; 
   	private const int formheight = 675; 
	private const int paddlelength = 30;  // The paddle ratio that Professor provided us were WAY TOO SMALL. So I increased it.
   	private const int paddlewidth = 150;
	private const int ground = formheight - 120; // The top of the control panel screen where buttons are located.
	private bool contact = false;
	private const int titlebottom = 50; // This is where the title ends.
	private const int horizantaladjustment = 8;
	private const int ball_radius = 12;

	// Coordinates of Ball
	private double real_coord_x = (double)( formwidth / 2 );
   	private double real_coord_y = (double)( ( ground - titlebottom ) / 2 );
   	private int init_coord_x;  
   	private int init_coord_y; 
	
	private double vertical_delta; 
	private double ball_horizontal_delta;
	private double ball_angle_radians;
	private double degrees = 0;
	algorithmlogic algorithms = new algorithmlogic();

	private int leftplayerscore = 0;
	private int rightplayerscore = 0;

	// Clocks
	private double distance_moved_per_refresh = 5.5;
	private const double graphicrefreshrate = 30.0;  
	private static System.Timers.Timer graphic_area_refresh_clock = new System.Timers.Timer();
	private const double update_rate = 27.75;  //Units are in Hz
	private static System.Timers.Timer control_clock = new System.Timers.Timer();
	private bool control_clock_active = false;  //Initial state: Clock is not active.

        //Left Paddle
   	private int left_paddle_x;  
   	private int left_paddle_y;
   	private int leftpaddle_verticaldelta = 0; // Paddle ONLY moves left/right

   	enum leftpaddlekeyposition {up,down}
   	private leftpaddlekeyposition move_up = leftpaddlekeyposition.up;
   	private leftpaddlekeyposition move_down = leftpaddlekeyposition.up;

	
	//Right Paddle
	private int right_paddle_x;  
   	private int right_paddle_y;
   	private int rightpaddle_verticaldelta = 0; // Paddle ONLY moves left/right

	enum rightpaddlekeyposition {up2,down2}
   	private rightpaddlekeyposition P2move_up = rightpaddlekeyposition.up2;
   	private rightpaddlekeyposition P2move_down = rightpaddlekeyposition.up2;
	

        private Color background_color = Color.Black;


	private Label top_bar = new Label() {
   		AutoSize = false,
		TextAlign = ContentAlignment.MiddleCenter,
        	Size = new Size ( formwidth , titlebottom ),
        	Location = new Point ( 0 , 0 ),
        	BackColor = Color.AliceBlue,
		ForeColor = Color.Black,
        	Font = new Font("Corona", 16),
		Text = "By: Raza Tahir"
   	};

	// DECLARE BUTTONS/LABELS' @ BOTTOM OF SCREEN TO START GAME
	// In the case of my computer, formheight - 95 == 575.
	private const int Button_Location_Y = formheight - 95;

	private NoFocusButton newgame = new NoFocusButton() {
		Text = "New Game",
		Size = new Size ( 150 , 50 ), // REMEMBER: it's Size( width , height ),
		Location = new Point ( 300 , Button_Location_Y ),
		BackColor = Color.Goldenrod,
		Font = new Font("Times New Roman", 20)
	};

	private NoFocusButton gamestate = new NoFocusButton() {
		Text = "Pause", // Initially, game hasn't started. But when user hits startgame, then it goes 
		Size = new Size ( 100 , 50 ),
		Location = new Point ( 500  , Button_Location_Y ),
		BackColor = Color.Silver,
		Font = new Font("Times New Roman", 20)
    	};

	private NoFocusButton exit = new NoFocusButton() {
		Text = "Exit",
		Size = new Size ( 100 , 50 ),
		Location = new Point ( 650 , Button_Location_Y ),
		BackColor = Color.Black,
		ForeColor = Color.Crimson,
		Font = new Font("Times New Roman", 20)
    	};


	private Label leftscore_text = new Label() {
		AutoSize = false,
		TextAlign = ContentAlignment.MiddleCenter,
		Dock = DockStyle.None,
		Location = new Point ( 25 , Button_Location_Y ),
        	Size = new Size ( 75 , 60 ), 
      		BackColor = Color.Transparent,
		ForeColor = Color.DarkSlateGray,
        	Font = new Font("Bitstream Charter", 18),
		Text = "Left Player: "
	};

	private Label leftscore_number = new Label() {
		AutoSize = false,
		TextAlign = ContentAlignment.MiddleCenter,
        	Size = new Size ( 100 , 60 ),
        	Location = new Point ( 125 , Button_Location_Y ),
        	BackColor = Color.White,
        	Font = new Font("Arial", 18)
	};

	private Label speed_text = new Label() {
		AutoSize = false,
		TextAlign = ContentAlignment.MiddleCenter,
		Dock = DockStyle.None,
		Location = new Point ( 775 , Button_Location_Y ),
        	Size = new Size ( 100 , 55 ),
      		BackColor = Color.Transparent,
		ForeColor = Color.MediumSeaGreen,
        	Font = new Font("Bitstream Charter", 18),
		Text = "Speed: "
	};

	private Label speed_number = new Label() {
		AutoSize = false,
		TextAlign = ContentAlignment.MiddleCenter,
		Location = new Point ( 900 , Button_Location_Y ),
        	Size = new Size ( 100 , 50 ),
      		BackColor = Color.White,
        	Font = new Font("Arial", 20),
	};

	private Label rightscore_text = new Label() {
		AutoSize = false,
		TextAlign = ContentAlignment.MiddleCenter,
		Dock = DockStyle.None,
		Location = new Point ( 1025 , Button_Location_Y ),
        	Size = new Size ( 75 , 60 ),
      		BackColor = Color.Transparent,
		ForeColor = Color.DarkSlateGray,
        	Font = new Font("Bitstream Charter", 18),
		Text = "Right Player: "
	};

	private Label rightscore_number = new Label() {
		AutoSize = false,
		TextAlign = ContentAlignment.MiddleCenter,
		Location = new Point ( 1125 , Button_Location_Y ),
        	Size = new Size ( 100 , 60 ),
      		BackColor = Color.White,
        	Font = new Font("Arial", 20),
	};


	
   	public Paddleframe() {   // Class Constructor
   	
		// Set the form
		Text = "Pong Game";
      		System.Console.WriteLine("formwidth = {0}. formheight = {1}.",formwidth,formheight);
      		Size = new Size(formwidth,formheight); 
      		BackColor = background_color;

		// GENERATES RANDOM LOCATION FOR BALL
		init_coord_x = (int)(real_coord_x);
      		init_coord_y = (int)(real_coord_y);

                // Random Direction
 
		
      		ball_angle_radians = algorithms.get_random_direction();
      
      		degrees = ball_angle_radians*180.0/System.Math.PI;

		// if degrees > 80 && degrees < 90 && degrees > 260 && degrees < 280 ) get a new a direction. 
		while ( degrees > 80 && degrees < 100 || degrees > 260 && degrees < 280 || ( degrees < -80 && degrees > -100 ) ) {
			algorithms = new algorithmlogic();
      			ball_angle_radians = algorithms.get_random_direction();
      			degrees = ball_angle_radians*180.0/System.Math.PI;
		}
	
		ball_horizontal_delta = distance_moved_per_refresh * System.Math.Cos(ball_angle_radians);
      		vertical_delta = distance_moved_per_refresh * System.Math.Sin(ball_angle_radians);
      		System.Console.WriteLine("Direction of ball: {0} Degrees, {1} Radians",
		Round ( degrees , 2) , Round( ball_angle_radians , 2) );
      		
	
		// Set clocks
		graphic_area_refresh_clock.Enabled = false;  
		graphic_area_refresh_clock.Elapsed += new ElapsedEventHandler(Updatedisplay);  

		control_clock.Enabled = false; //Initially the clock controlling the ball is stopped.
		control_clock.Elapsed += new ElapsedEventHandler(Updateball);

		Startgraphicclock(graphicrefreshrate);  
		//refreshrate is how many times per second the display area is re-painted.
		Startclock(update_rate);  

		

		//Make buttons visible
		Controls.Add(top_bar);
      		Controls.Add(newgame);
		Controls.Add(gamestate);
		Controls.Add(exit);

		Controls.Add(leftscore_text);
		Controls.Add(leftscore_number);
		Controls.Add(speed_text);
		Controls.Add(speed_number);
		Controls.Add(rightscore_number);
		Controls.Add(rightscore_text);

		//Initialize buttons
		newgame.Click += new EventHandler ( startgame );
		gamestate.Click += new EventHandler ( gamestatus );
      		exit.Click += new EventHandler(terminate);
                

                //Initialize paddle (near) the center of the form.
		left_paddle_x = paddlelength / 2;
		left_paddle_y = ( ground - titlebottom ) / 2;
	
		right_paddle_x = formwidth - ( 2 * paddlelength);
		right_paddle_y = ( ground - titlebottom ) / 2;
		
		KeyPreview = true;
		KeyUp += new KeyEventHandler(OnKeyUp);

		DoubleBuffered = true; 

   	}//End of constructor

	protected override void OnPaint(PaintEventArgs ee) {
		Graphics graph = ee.Graphics;
		BackColor = background_color;
	    
		// ball
		graph.FillEllipse (
			Brushes.Red, init_coord_x, init_coord_y, 2 * ball_radius, 2 * ball_radius 
		);

		// bottom box
		graph.FillRectangle(Brushes.Cornsilk , 0 , ground ,formwidth, 120); 

		// paddle1
		graph.FillRectangle(Brushes.Orange , left_paddle_x , left_paddle_y , paddlelength , paddlewidth );
		
		// paddle2
		graph.FillRectangle(Brushes.Orange , right_paddle_x , right_paddle_y , paddlelength , paddlewidth );

	     	// Display Numbers 
		leftscore_number.Text = leftplayerscore.ToString("0.");
		rightscore_number.Text = rightplayerscore.ToString("0.");
		speed_number.Text = distance_moved_per_refresh.ToString("0.00");
		
		base.OnPaint(ee);
	}

	
   	protected override bool ProcessCmdKey(ref Message msg, Keys KeyCode) {

		// VALUES ON GRAPHIC INTERFACE IS COUNTERINTUITIVE TO COORDINATE AXIS. Going DOWN means INCREASING Y
		if(KeyCode == Keys.W) {
			move_up = leftpaddlekeyposition.down;
			if ( move_down == leftpaddlekeyposition.up )
				leftpaddle_verticaldelta = -5; 
           	}

		else if(KeyCode == Keys.S) {
			move_down = leftpaddlekeyposition.down;
			if ( move_up == leftpaddlekeyposition.up )
				leftpaddle_verticaldelta = +5;
           	}

		// VALUES ON GRAPHIC INTERFACE IS COUNTERINTUITIVE TO COORDINATE AXIS. Going DOWN means INCREASING Y
		else if(KeyCode == Keys.O) {
			P2move_up = rightpaddlekeyposition.down2;
			if ( P2move_down == rightpaddlekeyposition.up2 )
				rightpaddle_verticaldelta = -5; 
           	}

		else if(KeyCode == Keys.L) {
			P2move_down = rightpaddlekeyposition.down2;
			if ( P2move_up == rightpaddlekeyposition.up2 )
				rightpaddle_verticaldelta = +5;
           	}

		else {
			System.Console.WriteLine("KeyCode = {0}.",KeyCode);
           	}

		right_paddle_y += rightpaddle_verticaldelta;
		left_paddle_y += leftpaddle_verticaldelta;
      		Invalidate();

      		return base.ProcessCmdKey(ref msg, KeyCode);

        }//End of method ProcessCmdKey



	private void OnKeyUp(object sender, KeyEventArgs e) {

		if(e.KeyCode == Keys.W) {
			move_up = leftpaddlekeyposition.up;
			if ( move_down == leftpaddlekeyposition.down )
				leftpaddle_verticaldelta = +5;
			else
                        	leftpaddle_verticaldelta = 0;
           	}

		else if(e.KeyCode == Keys.S) {   
			move_down = leftpaddlekeyposition.up;
			if ( move_up == leftpaddlekeyposition.down )
				leftpaddle_verticaldelta = -5;
			else
				leftpaddle_verticaldelta = 0;
		}

		if(e.KeyCode == Keys.O) {
			P2move_up = rightpaddlekeyposition.up2;
			if ( P2move_down == rightpaddlekeyposition.down2 )
				rightpaddle_verticaldelta = +5;
			else
                        	rightpaddle_verticaldelta = 0;
           	}

		else if(e.KeyCode == Keys.L) {   
			P2move_down = rightpaddlekeyposition.up2;
			if ( P2move_up == rightpaddlekeyposition.down2 )
				rightpaddle_verticaldelta = -5;
			else
				rightpaddle_verticaldelta = 0;
		}
		
		
		left_paddle_y += leftpaddle_verticaldelta;
		right_paddle_y += rightpaddle_verticaldelta;
		Invalidate();

       } //End of method OnKeyUp


	// Clocks

	protected void Startgraphicclock(double refreshrate) {

		double elapsedtimebetweentics;
		if(refreshrate < 1.0) refreshrate = 1.0; 
		elapsedtimebetweentics = 1000.0/refreshrate;  
		graphic_area_refresh_clock.Interval = (int)System.Math.Round(elapsedtimebetweentics);
		graphic_area_refresh_clock.Enabled = true;  

	}

	protected void Startclock(double updaterate) {

		double elapsedtimebetweenballmoves;
		if(updaterate < 1.0) updaterate = 1.0;  
		elapsedtimebetweenballmoves = 1000.0/updaterate;  
		control_clock.Interval = (int)System.Math.Round(elapsedtimebetweenballmoves);
 		control_clock.Enabled = false;   
 		control_clock_active = true;

	}

      protected void Updatedisplay(System.Object sender, ElapsedEventArgs evt) {

		Invalidate();  
		if ( !(control_clock_active) ) {
			graphic_area_refresh_clock.Enabled = false;
			System.Console.WriteLine("The graphical area is no longer refreshing.  You may close the window.");
		}

	}

	protected void Updateball(System.Object sender, ElapsedEventArgs evt) {  
		real_coord_x = real_coord_x + ball_horizontal_delta;
      		real_coord_y = real_coord_y - vertical_delta;  
      		init_coord_x = (int)System.Math.Round(real_coord_x);
      		init_coord_y = (int)System.Math.Round(real_coord_y);

		if( init_coord_x <= ( 0 ) ) { // if ball hits left "goal post"
			System.Console.WriteLine("Right Player gets a point!");			
			contact = true;
			rightplayerscore = rightplayerscore + 1; // if Left Player can't hit ball, then Right Player gets a point
		} 

		// player hits right goal post
		else if ( init_coord_x >= formwidth ) { 
			System.Console.WriteLine("Left Player gets a point!");
			leftplayerscore = leftplayerscore + 1;
			contact = true;
		}
		
		// ball has either hit top or gone above "top". NOTE THAT VALUE IS LOW
		else if( init_coord_y <= titlebottom ) {	
			vertical_delta = - vertical_delta;
		}

		// ball has either hit bottom or gone past "bottom".
		else if ( init_coord_y + 2 * ball_radius >= ground ) {
			vertical_delta = - vertical_delta;
		}

		
		// ball has hit left paddle: 
		else if ( init_coord_x >= 0 && init_coord_x <= ( left_paddle_x + paddlelength ) &&
			  init_coord_y >= left_paddle_y && init_coord_y <= ( left_paddle_y + paddlewidth ) ) {

			ball_horizontal_delta = - ball_horizontal_delta;
		}

		// ball has hit right paddle: 
		else if ( init_coord_x >= right_paddle_x && init_coord_x <= (right_paddle_x + paddlelength) &&
			  init_coord_y >= right_paddle_y && init_coord_y <= ( right_paddle_y + paddlewidth ) ) {

			ball_horizontal_delta = - ball_horizontal_delta;
		}


		// If Ball hits the the goal post and there are still some remaining balls left..Continue.
		if ( contact == true && (leftplayerscore + rightplayerscore) < 5 ) {
			contact = false;
			System.Console.WriteLine("Generating new random posiion...Done.");

		// new random direction for generated ball
			algorithms = new algorithmlogic();
      			ball_angle_radians = algorithms.get_random_direction();
  			degrees = ball_angle_radians*180.0/System.Math.PI;

			while ( ( degrees > 80 && degrees < 100 ) || ( degrees > 260 && degrees < 280 ) || ( degrees < -80 && degrees > -100 )) {
				algorithms = new algorithmlogic();
      				ball_angle_radians = algorithms.get_random_direction();
      				degrees = ball_angle_radians*180.0/System.Math.PI;		
			}

			ball_horizontal_delta = distance_moved_per_refresh * System.Math.Cos(ball_angle_radians);
      			vertical_delta = distance_moved_per_refresh * System.Math.Sin(ball_angle_radians);
	
			// new coordinates ( ball goes back to center ) 
			init_coord_y = ( ground - titlebottom ) / 2;
			init_coord_x = formwidth / 2;
			real_coord_y = ( ground - titlebottom ) / 2 ;
			real_coord_x = formwidth / 2;		
			distance_moved_per_refresh = distance_moved_per_refresh + 1.1;
			control_clock.Enabled = true;
			graphic_area_refresh_clock.Enabled = true;	
		
		}

		// GAME OVER. Ran out of balls
		if ( leftplayerscore + rightplayerscore == 5 ) {
			control_clock.Enabled = false;
			System.Console.WriteLine("The game has ended.");
			if ( leftplayerscore > rightplayerscore ) {
			System.Console.WriteLine("Left player has won!");
			}
			else System.Console.WriteLine("Right player has won!");
			System.Console.WriteLine("You can start a new game if you'd like, but that would erase your previous score" + "\n" 							+"If you wish to continue, press the New Game button");
		}
	} //End of method Updateball


	protected void startgame ( Object sender, EventArgs events ) {
	
		init_coord_y = ( ground - titlebottom ) / 2;
		init_coord_x = formwidth / 2;
		real_coord_y = ( ground - titlebottom ) / 2 ;
		real_coord_x = formwidth / 2;
		leftplayerscore = 0;
		rightplayerscore = 0;
		contact = false;
		left_paddle_x = paddlelength / 2;
		distance_moved_per_refresh = 5.5;

		vertical_delta = distance_moved_per_refresh * ( System.Math.PI / -2 );

		if ( control_clock.Enabled == false ) {
			control_clock.Enabled = true;
			control_clock_active = true;
			graphic_area_refresh_clock.Enabled = true;
			gamestate.Text = "Pause"; // game automatically starts upon click.
		}

		/*
		control_clock.Enabled = false;
		graphic_area_refresh_clock.Enabled = true;
		*/

	} // end of startprogram method.

	protected void gamestatus ( Object sender, EventArgs events ) {
	
		// The game has either yet to start, OR the user clicked on "Start" when the game is paused && clock.Enabled. 
		// Results of this if statement results in the ball moving.
		if ( control_clock.Enabled == false) { 
			gamestate.Text = "Pause";
			control_clock.Enabled = true;
			control_clock_active = true;
			graphic_area_refresh_clock.Enabled = true;

		}

		// The ball is stationary. When the user clicks on the button, the ball will move again.
		else {
			gamestate.Text = "Start";
			control_clock.Enabled = false;
		}

	} // end of pauseprogram method.

       protected void terminate (Object sender,EventArgs events) {

		System.Console.WriteLine("This program will end execution.");
		Close();

       }//End of terminate method

}//End of class Paddleframe

