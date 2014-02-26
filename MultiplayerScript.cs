using UnityEngine;
using System.Collections;

/// <summary>
/// All code is taken from Gamer To Game Developer Series 1
/// http://www.gamertogamedeveloper.com
/// No credit is taken myself
/// 
/// </summary>

public class MultiplayerScript : MonoBehaviour {
	// Variables start__________________________
	private string titleMessage = "GTGD Series 1 Prototype";
	private string connectToIP = "127.0.0.1";
	private int connectionPort = 26500;
	private bool useNAT = false;
	private string ipAddress;
	private string port;
	private int numberOfPlayers = 10;

	public string playerName;
	public string serverName;
	public string serverNameForClient;

	private bool iWantToSetupAServer = false;
	private bool iWantToConnectToAServer = false;

	// These ariables are used to define the main window
	private Rect connectionWindowRect;
	private int connectionWindowWidth = 400;
	private int connectionWindowHeight = 280;
	private int buttonHeight = 60;
	private int leftIndent;
	private int topIndent;

	// These variables are used to define the server
	// shutdown window
	private Rect serverDisWindowRect;
	private int serverDisWindowWidth = 300;
	private int serverDisWindowHeight = 150;
	private int serverDisWindowLeftIndent = 10;
	private int serverDisWindowTopIndent = 10;


	// These variables are used to define the client
	// disconnect window
	private Rect clientDisWindowRect;
	private int clientDisWindowWidth = 300;
	private int clientDisWindowHeight = 170;
	private bool showDisconnectWindow = false;

	// Variables end____________________________


	// Use this for initialization
	void Start () 
	{
		// Load the last used server name from registry
		// and if the server name is blank then use "Server"
		// as the default name
		serverName = PlayerPrefs.GetString("serverName");
		if(serverName == "")
		{
			serverName = "Server";
		}

		// Load the last used player name from the registry
		// and if the player name is blank then use "Player"
		// as the default name
		playerName = PlayerPrefs.GetString("Player");
		if(playerName == "")
		{
			playerName = "Player";
		}
	}


	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			showDisconnectWindow = !showDisconnectWindow;
		}
	}


	void ConnectWindow(int windowID)
	{
		// Space

		GUILayout.Space(15); // pixels
		
		// When the player launches the game they have the option
		// to create a server or join a server. The variables
		// iWantToSetupAServer and iWantToConnectToAServer start as
		// false so the player is presented with two buttons
		// "Setup my server" and "Connect to a server"
		if(iWantToSetupAServer == false && iWantToConnectToAServer == false)
		{
			if(GUILayout.Button("Setup a server", GUILayout.Height(buttonHeight)))
			{
				iWantToSetupAServer = true;
			}

			GUILayout.Space(10);

			if(GUILayout.Button("Connect to a server", GUILayout.Height(buttonHeight)))
			{
				iWantToConnectToAServer = true;
			}

			GUILayout.Space(10);

			if(Application.isWebPlayer == false && Application.isEditor == false)
			{
				if(GUILayout.Button("Exit Prototype", GUILayout.Height(buttonHeight)))
				{
					Application.Quit();
				}
			}
		}

		if(iWantToSetupAServer == true)
		{
			// The user can type a name for their server into the text field
			GUILayout.Label("Enter a name for your server");
			serverName = GUILayout.TextField(serverName);

			GUILayout.Space(5);

			// The user can type in the Port number for their server
			// into the text field. We defined a default value above in the
			// variables as 26500
			GUILayout.Label("Server Port");
			connectionPort = int.Parse (GUILayout.TextField(connectionPort.ToString()));

			GUILayout.Space(10);

			if(GUILayout.Button("Start my own server", GUILayout.Height(30)))
			{
				// Create the server
				Network.InitializeServer(numberOfPlayers, connectionPort, useNAT);

				// Save the serverName using PlayerPrefs
				PlayerPrefs.SetString("serverName", serverName);

				iWantToSetupAServer = false;
			}

			if(GUILayout.Button("Go back", GUILayout.Height(30)))
			{
				iWantToSetupAServer = false;
			}
		}

		if(iWantToConnectToAServer == true)
		{
			// The user can type their player name into the text field
			GUILayout.Label("Enter your player name");
			playerName = GUILayout.TextField(playerName);

			GUILayout.Space (10);

			// The player can type the IP address for the server that
			// they want to connect to into the text field
			GUILayout.Label("Type in server IP");
			connectToIP = GUILayout.TextField(connectToIP);

			GUILayout.Space (5);

			// The player can type in the port number for the server
			// that they want to connect to into the text field
			GUILayout.Label ("Type in server port");
			connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString()));

			GUILayout.Space (5);

			// The player clicks on this button to establish a connection
			if(GUILayout.Button("Connect", GUILayout.Height(25)))
			{
				// Ensure that the player cannot join a game with an empty name
				if(playerName == "")
				{
					playerName = "Player";
				}

				// If the player has a name that is not empty, then attempt to
				// join the server
				if(playerName != "")
				{
					// Connect to a server with the IP address contained in connectToIP
					// and the port number contained in connectionPort
					Network.Connect(connectToIP, connectionPort);
					PlayerPrefs.SetString("playerName", playerName);
				}
			}

			if(GUILayout.Button("Go back", GUILayout.Height(25)))
			{
				iWantToConnectToAServer = false;
			}
		}
	}


	void ServerDisconnectWindow(int windowID)
	{
		// Show the server name and number of players connected.
		GUILayout.Label("Server name: " + serverName);
		GUILayout.Label("Number of players connected: " + Network.connections.Length);

		// If there is at least one connection then show the
		// average ping
		if(Network.connections.Length >=1)
		{
			GUILayout.Label("Ping: " + Network.GetAveragePing(Network.connections[0]));
		}

		// Shutdown the server if the user clicks on the disconnect bar
		if(GUILayout.Button("Shut down server"))
		{
			Network.Disconnect();
		}
		// Time of video: 1:14:45
	}


	void ClientDisconnectWindow(int windowID)
	{
		// Show the player the server they are connected to and the average ping of their
		// connection
		GUILayout.Label("Connected to server: " + serverName);
		GUILayout.Label("Ping: " + Network.GetAveragePing(Network.connections[0]));

		GUILayout.Space(7);

		// The player disconnects from the server when they press the "Disconnect" button
		if(GUILayout.Button("Disconnect", GUILayout.Height(25)))
		{
			Network.Disconnect();
		}

		GUILayout.Space(5);

		// This button allows the player using a webplayer who has gone full screen
		// to be able to return to the game. Pressing escape in full screen does not
		// help as that just exits full screen.
		if(GUILayout.Button("Return to game", GUILayout.Height (25)))
		{
			showDisconnectWindow = false;
		}
	}


	void OnDisconnectedFromServer()
	{
		// If a player loses the connection or leaves the scene then the level is restarted
		// on their computer
		Application.LoadLevel(Application.loadedLevel);
	}


	void OnPlayerDisconnected(NetworkPlayer networkPlayer)
	{
		// When a player disconnects from the server delete them accross the network along with their
		// RCPs so that other players no longer see them
		Network.RemoveRPCs(networkPlayer);
		Network.DestroyPlayerObjects(networkPlayer);
	}


	void OnPlayerConnected(NetworkPlayer networkPlayer)
	{
		networkView.RPC("TellPlayerServerName", networkPlayer, serverName);
	}


	void OnGUI()
	{
		// If the player is disconnected then run the ConnectWindow function
		if(Network.peerType == NetworkPeerType.Disconnected)
		{
			// Determine the position of the window based on the width and
			// height of the screen. The window will be placed in the middle
			// of the screen.
			leftIndent = (Screen.width - connectionWindowWidth) / 2;
			topIndent = (Screen.height - connectionWindowHeight) / 2;
			connectionWindowRect = new Rect(leftIndent, topIndent, connectionWindowWidth, 
			                                connectionWindowHeight);

			connectionWindowRect = GUILayout.Window(0, connectionWindowRect, ConnectWindow,
			                                        titleMessage);
		}

		// If the game is running as a server then run the ServerDisconnect
		// function
		if(Network.peerType == NetworkPeerType.Server)
		{
			// Defining the Rect for the server's disconnect window
			serverDisWindowRect = new Rect(serverDisWindowLeftIndent, serverDisWindowTopIndent,
			                               serverDisWindowWidth, serverDisWindowHeight);
			serverDisWindowRect = GUILayout.Window(1, serverDisWindowRect, ServerDisconnectWindow, "");
		}

		// If  the connection type is a client then show a window that allows them to
		// disconnect from the server
		if(Network.peerType == NetworkPeerType.Client && showDisconnectWindow)
		{
			//
			leftIndent = (Screen.width - clientDisWindowWidth) / 2;
			topIndent = (Screen.height - clientDisWindowHeight) / 2;
			clientDisWindowRect = new Rect(leftIndent, topIndent, clientDisWindowWidth, 
			                               clientDisWindowHeight);
			clientDisWindowRect = GUILayout.Window(1, clientDisWindowRect, ClientDisconnectWindow, "");
		}
	}


	// Used to tell the MultiplayerScript in connected players the serverName. Otherwise the 
	// players connecting would not be able to see the name of the server. 
	[RPC]
	void TellPlayerServerName(string servername)
	{
		serverName = servername;

	}
}
