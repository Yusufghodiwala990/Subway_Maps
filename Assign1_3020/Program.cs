/*----------------------------------------------------------------------*/

/*This console application uses Concepts in Graphs/Graph theory to manipulate vertexes and edges in the form of Subway Stations
 * to be able to apply various Graph Theory techniques. Stations are stored in a List while connections of stations are stored
   in a Linked-list. This is the Adjacency List implementation of a Graph. We can perform meaningful operations such as 
 * Breadth-First Search and Depth-First Search to gather useful information. */

/*----------------------------------------------------------------------*/

//Assignment 1 by Yusuf Ghodiwala (0683640)




using System;
using System.Collections.Generic;

public enum Colour{RED,GREEN,BLUE,YELLOW}      // read-only variables for colours, i'm using 4 of them to differentiate lines.


// Setting up Node class. Node in this application will be used to store Connections of a station in the form of a LinkedList.

/* NOTE: I'm storing the Colour Lines in a List to avoid duplication. Each Adjacent Station(Node) will have a list of colours that
 *       will identify which Line passes through between the two stations. If more Lines are added, i simply just add it to the List
 *        and NOT more nodes */

public class Node
{
    public Station Connection { get; set; }   // Adjacent station (Connection)
    public List<Colour> Line { get; set; } // list of colours
    public Node Next { get; set; }         // reference to the next adjacent station

    //Constructor
    // Storing the Connection, a List of Colours and a reference to the next adjacent station.
    public Node(Station connection, Node next)
    {
        Connection = connection;
        Line = new List<Colour>(4); // i add the colour to the list in the insertConnection method, not here.
        Next = next;

    }
}





/* Setting up Station class. Each station will have a name, a bool variable called Visited(which will be used in performing BFS and DFS)
 *  A linked-list to store adjacent stations (stations it's connected to) as an instance of the node class */
 
public class Station
{
    public string Name { get; set; }
    public bool Visited { get; set; }
    public Node E { get; set; }           // head node of the linked-list

    // Constructor
    public Station(string name)
    {
        Name = name;
        Visited = false;           //initialized to false
        E = new Node(null,null);
        
    }

}


// SubwayMap
class SubwayMap
{
    private List<Station> S;              // List of stations
    public SubwayMap()
    {
        S = new List<Station>(20);      // initialized to size of 20
    }



    //Method : FindStation
    //Parameters: Takes the string name from the user
    //Return Type : int
    //Description: This method takes a string to find the station in the List. If it exists, it returns the index of station in the
    //              in the list. If it doesn't, returns -1

    public int FindStation(string name)
    {
        int i;
        for (i = 0; i < S.Count; i++)
        {
            if (S[i].Name.Equals(name))            // using C#'s List class' method. 
                return i;
        }
        return -1;
       
    }



    //Method : InsertStation
    //Parameters: Takes the string name from the user
    //Return Type : void
    //Description: This method takes a string(station) from the user to add it to the list. 

    public void InsertStation(string name)
    {
        if (FindStation(name) != -1)             //calling FindStation method to find out if it already exists.
            Console.WriteLine("Sorry, such a station already exists!");
        else
        {
            Station s = new Station(name);     
            S.Add(s);                          // else, adding it to the List.
            Console.WriteLine("{0} was added", name);
        }

    }

    //Method : RemoveStation
    //Parameters: Takes the string name from the user
    //Return Type : void
    //Description: This method takes a string to remove a station from the List. Connections To and From that station 
    //             shall also be removed

    public void RemoveStation(string name)
    {
        int i;
        if ((i = FindStation(name)) > -1) // if the station exists
        {
            for (int j = 0; j < S.Count; j++)       // going through the entire list
            {
                Node curr = S[j].E;                // setting a reference to the adjacent linked-list of each station in the list

              // going through the linked-list. If we find a node that has the same name of the station being removed, delete the node

                while (curr.Next != null)        
                {
                    if (curr.Next.Connection.Name == name)        
                    {
                        curr.Next = curr.Next.Next;

                        break;                   // in my implementation i won't have duplicate nodes

                    }
                    curr = curr.Next;

                }
                


            }
            // now removing the station.
            S.RemoveAt(i); 
            Console.WriteLine("{0} has been removed", name);


        }
            // if the station to be deleted doesn't exist
        else
            Console.WriteLine("Sorry {0} doesn't exist", name);
        


    }

    //Method : InsertConnection
    //Parameters: Takes the strings for two stations and the Colour for the Line between them(edge).
    //Return Type : void
    //Description: This method will add a connection between two stations specified by Colour. Since this graph is Undirected
    //             we shall add both stations to each other's adjacent linked-list. Multiple connections can be added between 
    //             two stations differentiated by the colour of their line.

    public void InsertConnection(string name1, string name2, Colour c)
    {
        int i, j;  //i is station1, j is station2
        if ((i = FindStation(name1)) > -1 && (j = FindStation(name2)) > -1)    // checking if they exist
        {

            Node curr = S[i].E;              // reference to station1's adjacent linked-list
            Node curr2 = S[j].E;            //  reference to station2's adjacent linked-list
            bool connAdded = false;         // bool variable to check if a station has been added to an adjacent linked-list


            // if there are no adjacent stations(connections) from station1
            if (curr.Next == null)
            {
                curr.Next = new Node(S[j], null);     // creating an adjacent node
                curr.Next.Line.Add(c);                // adding Colour c into the List

            }
            else
            {
                // iterating through the linked-list of station1 to see if station2 already exists in it
                while (curr.Next != null && !connAdded)
                {
                    curr = curr.Next;
                    if (curr.Connection.Name == name2)  // if an existing adjacent station is found to connect to
                        if (curr.Line.Contains(c))      // if the color line already exists, using C# list class method.
                        {
                            Console.WriteLine("Sorry, {0} line already exists with these two stations", c);
                            connAdded = true;   // connection already existed
                            return;             // we exit the method
                        }
                        else 
                        {                          // else, add the Colour to the list
                            curr.Line.Add(c);
                            connAdded = true;     // connection has been added

                        }

                    
                }

                if (!connAdded)              // if we didn't find station2 in station1's linked-list, then it has to be inserted
                {                            // as a new node.
                    curr.Next = new Node(S[j], null);
                    curr.Next.Line.Add(c);            // adding the Colour to the list
                }


            }

            // we need to add the other way around too  ( adding station1 as an adjacent station to station2's linked-list
            connAdded = false;
            if (curr2.Next == null)           // curr2 is a reference to station2's adj linked-list.
            {
                curr2.Next = new Node(S[i], null);
                curr2.Next.Line.Add(c);
                Console.WriteLine("\n{0} line was added between {1} and {2}", c, S[i].Name, S[j].Name);
            }

            else
            {
                // iterating through the linked-list of station2 to see if station1 already exists in it
                while (curr2.Next != null && !connAdded)
                {
                    curr2 = curr2.Next;
                    if (curr2.Connection.Name == name1)  // if an existing adjacent station is found to connect to
                        if (curr2.Line.Contains(c))      // if the color line already exists
                        {
                            Console.WriteLine("Sorry, {0} line already exists with these two stations", c);
                            connAdded = true;   // connection already exists
                        }
                        else
                        {
                            curr2.Line.Add(c);             // if station1 has been found and the connection doesn't exist, add it.
                            connAdded = true;
                            Console.WriteLine("\n{0} line was added between {1} and {2}", c, S[i].Name, S[j].Name);
                        }
                    
                }

                if (!connAdded)              // if we didn't find station1 in station2's linked-list, then it has to be inserted
                {                                      // as a new node.     
                    curr2.Next = new Node(S[i], null);
                    curr2.Next.Line.Add(c);
                    Console.WriteLine("\n{0} line was added between {1} and {2}", c, S[i].Name, S[j].Name);

                }

            }
           
        }
            // else the stations don't exist
        else 
            Console.WriteLine("Such station/stations don't exist");
    }
    



    //Method : RemoveConnection
    //Parameters: Takes the strings for two stations and the Colour for the Line between them(edge).
    //Return Type : void
    //Description: This method will remove a connection between two stations specified by Colour. Since this graph is Undirected
    //             we shall remove the connection specified by Colour from both stations in each other's adjacent linked-list.

    public void RemoveConnection(string name1, string name2, Colour c)
    {
         int i, j;
         if ((i = FindStation(name1)) > -1 && (j = FindStation(name2)) > -1)         // checking if the stations exist.
         {
             Node curr = S[i].E;                  // reference to station1's adjacent linked-list
             Node curr2 = S[j].E;                 // reference to station2's adjacent linked-list
             bool connRemoved = false;            // bool variable to check if a connection has been removed from adj linked-list


             // iterating through station1's adj linked-list

             while (curr.Next != null && !connRemoved)
             {
                 if (curr.Next.Connection.Name == name2)         // if we find station2
                 {
                     if (curr.Next.Line.Contains(c))            // if we find the connection specified by colour in the list Line
                     {
                         curr.Next.Line.Remove(c);              // removing the Colour from the list Line.
                         connRemoved = true;
                     }
                     if (curr.Next.Line.Count == 0) //if we removed the last connection between two stations, remove the node as well
                     {
                         curr.Next = curr.Next.Next;
                         break;
                     }

                 }

                 curr = curr.Next;
             }

             // if bool variable is still false
             if (!connRemoved)
             {
                 Console.WriteLine("No such connection exists between these two stations to remove");
                 return;          // we exit right away, no need to check the other stations adj linked-list 
                                  // because if it didn't exist in this list, it doesn't exist in the other one as well.
             }


             connRemoved = false; // for 2nd station we do the same thing (if there is a connection that needs to be removed)
             while (curr2.Next != null && !connRemoved)
             {
                 if (curr2.Next.Connection.Name == name1)
                 {
                     if (curr2.Next.Line.Contains(c))
                     {
                         curr2.Next.Line.Remove(c);
                         connRemoved = true;
                         Console.WriteLine("\n{0} line was removed between {1} and {2}", c, S[i].Name, S[j].Name);
                         
                     }
                     if (curr2.Next.Line.Count == 0)
                     {
                         curr2.Next = curr2.Next.Next;
                         break;
                     }
                 }

                 curr2 = curr2.Next;
             }
             if (!connRemoved)
                 Console.WriteLine("");
         }


    }


    //Method : PrintConnections
    //Parameters: No parameters
    //Return Type : void
    //Description: This method will print all the connections in the subway map/graph.

    public void printConnections()
    {
        for (int i = 0; i < S.Count; i++)              // iterating through the station list
        {
            Node curr = S[i].E;                       // reference to each station's adj linked-list
            while (curr.Next != null)                 // iterating through adj linked-list
            {
                for (int j = 0; j < curr.Next.Line.Count; j++)       // printing each Colour of an adj station as well.
                {
                    Console.WriteLine("{0} to {1} via [{2}] line", S[i].Name, curr.Next.Connection.Name, curr.Next.Line[j]);
                }
                curr = curr.Next;

            }
        }

    }

    //Method : PrintConnections
    //Parameters: No parameters
    //Return Type : void
    //Description: This method will print all the stations in the subway map/graph.

    public void printStations()
    {
        if (S.Count == 0)
            Console.WriteLine("There are no stations to print!");
        else
        {
            for (int i = 0; i < S.Count; i++)       // iterating through the station list.
            {
                Console.WriteLine(S[i].Name);
            }
        }

    }


    //Method : FastestRoute
    //Parameters: strings for Origin and Destination
    //Return Type : void
    //Description: This method is an implementation of Breadth-First Search which applies the property that when conducting
    //             a BFS in a graph, the first time you visit a node IS the shortest path to it from the node you started from.
    //             With the help of this property, we can calculate the Fastest Route from a start and end in the Subway Map.
    //             This method also Calls ConstructPath to build a path from the origin to destination using the BFS done in this
    //             method.


    // used https://youtu.be/oDqjPvD54Ss and https://en.wikipedia.org/wiki/Breadth-first_search for the explanation of the Algorithm


    public void FastestRoute(string From, string To)
    {
        int i,j;                                        // i refers to origin(start), j destination(end)


        Station[] prev;                                 // an array to keep track of the node we came from (previous node)
                                                        // e.g if we are currently on Node B which is in Index pos 2 in the list
                                                        // Then we will store the node we came from to reach B, let's say Node A at 
                                                        // Index 2 in prev array
                                                        //  that way we can easily look up two things, one, what node is index 2
                                                        //   from the List and two, which node did we come from to get to the node
                                                        //    in index two of prev array
                                                        //
                                                        //    prev[2] = Node A -> this means Node A is what we came from to reach Node
                                                        //              B (prev[2])



        prev = new Station[S.Count];                      // initializing the array

        List<Station> route = new List<Station>(S.Count);     // creating a list to formulate the fastest_route

        for (i = 0; i < prev.Length; i++)
            prev[i] = null;   // marking every element as null
        for (i = 0; i < S.Count; i++)         // marking visited variable of every station as false.
            S[i].Visited = false;

        
        i = FindStation(From);               // storing indexes of stations i and j, (origin & destination)
        j = FindStation(To);

        if (i == -1 || j == -1)                                // if they don't exist
            Console.WriteLine("No such station/stations exists");
         
        
        else                                          // else, perform a BFS from the origin(From)
        {
            
            Queue<Station> Q = new Queue<Station>();          // creating a queue using C#'s implementation of queue.
            Station s = S[i];                                  
            Q.Enqueue(s);                                    // inserting the Origin station into the queue
            s.Visited = true;                                // marking visited as true

            while (Q.Count != 0)                           // this loop will visit all adjacent stations of particular station in 
            {                                              //  in FIFO manner
                s = Q.Dequeue();

                Node curr = s.E;                         // reference to the adj linked-list of a station

                while (curr.Next != null)             //iterating through the adj linked-list
                {
                    if (curr.Next.Connection.Visited == false)            // if we haven't visited adj station yet.
                    {
                        Q.Enqueue(curr.Next.Connection);                 // place the adj station in the queue
                        curr.Next.Connection.Visited = true;             // mark the adj station as visited

                        prev[FindStation(curr.Next.Connection.Name)] = s;    // s is the station whose linkedList we explored. So
                                                                             // so any station in the adjacent linked-list of
                                                                             // s, will have s as the previous station we came from.
                        
                        // we store s in the index of the adj station from the list 'S' by using FindStation().


                    }


                    // while iterating thorugh adj stations of any station, if we come across our destination
                    if (curr.Next.Connection.Name == To)
                    {
                        ConstructPath(prev,curr,From);       // we found the destination, we need to construct a path by calling
                                                             // ConstructPath.     
                        return;                              // we exit the method because we no longer need to perform any more BFS.
                    }

                 curr = curr.Next;
                }
                
            }

           

        }
        Console.WriteLine("There is no route that can take you to {0}", To);

    }

    //Method : ConstructPath
    //Parameters: prev[] from FastestRoute, curr was our destination station, From is origin.
    //Return Type : void
    //Description: This method will use the variables from FastestRoute to build a route from Origin-Destination and print it.


    

    public void ConstructPath(Station[] prev, Node curr, string From)
    {
        List<Station> track = new List<Station>();               //creating a list to store the track we need to take.
        track.Add(curr.Next.Connection);                         // we add curr (destination) to the list.
        Station s;
        s = prev[FindStation(curr.Next.Connection.Name)];          // storing the station we came from to get to the destination.


        // moving backwards to find the origin (From) since we have an array of all the previous stations.

        while (s.Name != From)
        {
            track.Add(s);                      // adding s (the previous station), to the list
            s = prev[FindStation(s.Name)];    // once s (the previous station) is added, we find it's previous station.

        }
        track.Add(s);               // once we find our origin, we add it to the list as well
        track.Reverse();            // since we worked backwards(by finding previous stations of each station), the list 
                                    // will appear from destination to origin. Thus we use C#'s Reverse() method to flip it.

    
        Node head;                // reference to adj linkedlist of the stations in track.

        Console.WriteLine("\nHere is your fastest route...");

        // looping through the list
        for (int i = 0; i < track.Count-1; i++)
        {
            head = track[i].E;         // placing head on stations adj linkedlist


            // i could have probably just printed the list without going through the adj linked-list but :/


            while (head.Next.Connection.Name != track[i + 1].Name)        // i+1 is next station you need to visit, so we look for it
            {                                                             // in the adj linked-list
                head = head.Next;
            }

            Console.WriteLine("\nGo from {0} -> {1} via these connections:",track[i].Name,track[i + 1].Name);
            foreach (Colour q in head.Next.Line) // printing all the colour lines that you can use to get to the next station(i -> i+1)
                Console.WriteLine(q);

        }
        Console.WriteLine("\nYou will have reached {0} in the fastest way possible", track[track.Count - 1].Name);
            
           
       


       


    }

    //Method : findCritical()
    //Parameters: No parameters
    //Return Type : void
    //Description: This method will print all the criticalConnections in the subway map/graph. If these connections are removed
    //              then the graph will split into 2 parts because there are no alternative ways to get to a particular station
    //             This method also uses the recursive Depth-First Search method to perform DFS.

    // Source : I used Tarjan's algorithm wikipedia page to explain the concept of Strongly-Connected Components and finding
    //           back-edges using lowtimes and visitedTimes arrays. https://en.wikipedia.org/wiki/Bridge_(graph_theory)
    //          and this video https://www.youtube.com/watch?v=ECKTyseo2H8 for visual explanation.

    public void findCritical()
    {
        int[] lowtimes = new int[S.Count];        // this array will store back-edges of a node by checking if 
                                                  // the current node(station) is connected to a node(that has 
                                                  //  already been visited) that it's parent was connected 
                                                  // to, i.e a triangle or an alt way of getting to the current node. This is done
                                                  //  by storing the index of the node that it's parent was connected to in lowtimes[]
         
                                                  // e.g    if there are nodes A, B, C then we start DFS from A, we explore the nodes
                                                  //        and reach C, B is the parent. When we explore adj nodes of C, we find B
                                                  //        but it's the parent so we move to A, another adj node. But it has already
                                                  //        been visited, so we know for a fact that i can go to A using B, if the
                                                  //        connection between A and C is removed. So that is a back-edge and
                                                  //        not a critical connection. We are only looking for node which
                                                  //        don't have back-edges
  

        int[] visitedTimes = new int[S.Count];      // this array will store the time which we explored a node(station)
                                                    //  in the recursive call. But time will be used a post-increment. So
                                                    //   if we started at Station A it's time would be 0 but if we perform 
                                                    //    a recursive call again & reach B then B's visitedTime would be 1.


        int time = 0;                              // this will be used to manipulate which recursive call we explored a node and in
                                                   //  the case of a back-edge, we store with which node we have back-edge to.

        for (int i = 0; i < S.Count; i++)        // marking every station visited to false
            S[i].Visited = false;

        List<Station> critCon = new List<Station>();   // creating a list to store all the criticalConnections

        DepthFirstSearch(S[0],null,critCon,lowtimes,visitedTimes,time);       // calling DFS method to start from Station[0] 
                                                                              // and passing all the variables from this method
                                                                              // since it's recursive.

        Console.WriteLine("Removing these connection/connections will break the map into 2 parts\n");
        for (int i = 0; i < critCon.Count; i+=2)
        {
            Console.WriteLine("{0} <-> {1}\n", critCon[i].Name, critCon[i + 1].Name);
            
        }
        
    }



    public void DepthFirstSearch(Station v, Station parent, List<Station> critCon, int[] lowtimes, int[] visitedTimes, int time)
    {
      

        v.Visited = true;                    // v is the current station we're at. So we mark it visited.

        visitedTimes[FindStation(v.Name)] = lowtimes[FindStation(v.Name)] = time++;  // we increment both lowtimes and visitedtimes of
                                                                                     // the current station at it's apt index
        Station w;        // will be used to store the adj station of v

        Node curr = v.E;          // reference to adj of v

        // looping through adj linked-list of v
        while (curr.Next != null)
        {
            w = curr.Next.Connection;
            if (w == parent)  // if we came from the parent
            {
                goto Continue;   // skip this iteration because we already have explored the parent, since we came from it.
            }
            if (!w.Visited)     // if we haven't visited v's adj stations yet
            {
                DepthFirstSearch(w, v, critCon, lowtimes, visitedTimes,time);     // we do a DFS on w, but this time w becomes 
                                                                                  // v(current) station and v becomes parent.
                 
                 // if there is a back-edge, we update low times of v by comparing the smallest low times between w and v.               
                lowtimes[FindStation(v.Name)] = Math.Min(lowtimes[FindStation(v.Name)],lowtimes[FindStation(w.Name)]); 
                           
                
                // since we updated the low times of v by checking the w's low-times. 
                //  if there was a backedge, then low times of v would be greater than visitedTimes of w but if that isn't
                //  the case, then it is a criticalConnection and there are no alt ways to get to from w to v.
                if (visitedTimes[FindStation(v.Name)] < lowtimes[FindStation(w.Name)])
                {
                    critCon.Add(v);     // we add both w and v in the list
                    critCon.Add(w);
                }
            }
            else           // if w has been visited, and it's not the parent, update it's lowtimes because there is a back-edge
                lowtimes[FindStation(v.Name)] = Math.Min(lowtimes[FindStation(v.Name)], visitedTimes[FindStation(w.Name)]);
               
           
           Continue: 
            curr = curr.Next;
        }

       
    }

}




// Main program for an interface to interact with the Program
class main
{
    public static void Main(string[] args)
    {

        SubwayMap s;
        s = new SubwayMap();

        // try-catch block to catch exceptions
        try{
         Console.WriteLine("\nHere are your options:\n1. Add Station\n2. Remove Station\n3. Insert Connection\n4. Remove Connection\n5. Fastest Route between 2 stations\n6. Critical Connections\n7. Print Connections\n8. Print Stations\n9. Exit");
                int choice = Convert.ToInt32(Console.ReadLine());
               
                    Console.Clear();
                    while (choice != 9)         // exit loop when 9 is selected as the option
                    {
                        switch (choice)
                        {

                            case 1:
                                Console.WriteLine("Enter name");
                                s.InsertStation(Console.ReadLine());
                                break;
                            case 2:
                                Console.WriteLine("Enter name");
                                s.RemoveStation(Console.ReadLine());
                                break;
                            case 3:
                                string name1, name2;
                                Colour c;
                                Console.Write("Enter Station 1 : ");
                                name1 = Console.ReadLine();
                                Console.Write("Enter Station 2: ");
                                name2 = Console.ReadLine();
                                Console.WriteLine("Colour options are;");
                                Console.WriteLine("1.{0}", Colour.BLUE);
                                Console.WriteLine("2.{0}", Colour.GREEN);
                                Console.WriteLine("3.{0}", Colour.RED);
                                Console.WriteLine("4.{0}", Colour.YELLOW);

                                Console.WriteLine("\nPlease enter the colour(number) you want");
                                int p = Convert.ToInt32(Console.ReadLine());

                                switch (p)
                                {
                                    case 1:
                                        s.InsertConnection(name1, name2, Colour.BLUE);
                                        break;
                                    case 2:
                                        s.InsertConnection(name1, name2, Colour.GREEN);
                                        break;
                                    case 3:
                                        s.InsertConnection(name1, name2, Colour.RED);
                                        break;
                                    case 4:
                                        s.InsertConnection(name1, name2, Colour.YELLOW);
                                        break;
                                    default:
                                        Console.WriteLine("Invalid choice");

                                        break;

                                }
                                break;

                            case 4:
                                Console.WriteLine("Enter Station 1");
                                name1 = Console.ReadLine();

                                Console.WriteLine("Enter Station 2");
                                name2 = Console.ReadLine();

                                Console.WriteLine("Colour options are;");
                                Console.WriteLine("1.{0}", Colour.BLUE);
                                Console.WriteLine("2.{0}", Colour.GREEN);
                                Console.WriteLine("3.{0}", Colour.RED);
                                Console.WriteLine("4.{0}", Colour.YELLOW);

                                Console.WriteLine("\nPlease enter the colour(number) you wish to remove");
                                p = Convert.ToInt32(Console.ReadLine());

                                switch (p)
                                {
                                    case 1:
                                        s.RemoveConnection(name1, name2, Colour.BLUE);
                                        break;
                                    case 2:
                                        s.RemoveConnection(name1, name2, Colour.GREEN);
                                        break;
                                    case 3:
                                        s.RemoveConnection(name1, name2, Colour.RED);
                                        break;
                                    case 4:
                                        s.RemoveConnection(name1, name2, Colour.YELLOW);
                                        break;
                                    default:
                                        Console.WriteLine("Invalid choice");

                                        break;
                                }

                                break;
                            case 5:
                                Console.WriteLine("Enter Origin");
                                name1 = Console.ReadLine();
                                Console.WriteLine("Enter Destination");
                                name2 = Console.ReadLine();
                                s.FastestRoute(name1, name2);
                                break;
                            case 6:
                                s.findCritical();
                                break;
                            case 7:
                                s.printConnections();
                                break;
                            case 8:
                                s.printStations();
                                break;
                            case 9:
                                return;
                            default:
                                Console.WriteLine("Not a valid option");
                                break;
                        }
                        Console.WriteLine("\nHere are your options:\n1. Add Station\n2. Remove Station\n3. Insert Connection\n4. Remove Connection\n5. Fastest Route between 2 stations\n6. Critical Connections\n7. Print Connections\n8. Print Stations\n9. Exit");
                        choice = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();

                    }
                }
                catch(FormatException e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();


                }
                Console.WriteLine("See you next time!");
                Console.ReadLine();
            }
     

 }
