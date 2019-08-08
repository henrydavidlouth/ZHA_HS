HS_METHODS.s
//a class of the geometry generation and transform methods 
/////for massing scale
 ///////////////////


/// <summary>
        /// establish the halfedge which will be used for beginning the miminal pologon drawing/calculating
        ///sort all the edges and get the angle which is least
        /// </summary>
  void SetVertexMinHeInt(){

    _vertMinAngHeInts = new List <int> ();

    //////**NOTE ASSIGNMENT IS BY VERTWEX this methoid

    //load dummy info
    for (int j = 0; j < _hemesh.Vertices.Count; j++){
      _vertMinAngHeInts.Add(0);
    }

    //reset min until circulation of vertex complete
    for (int j = 0; j < _activeVertexSelectionInts.Count; j++){

      HeMesh3d.Vertex v = _hemesh.Vertices[_activeVertexSelectionInts[j]];

      double curMin = 360;
      int curMinInt = 0;

      //do a method for Y ints //conditional 1 methods fot Y nodes
      if( _isYVertices[v.Index]){
        foreach(HeMesh3d.Halfedge he in v.OutgoingHalfedges){

          double ang = he.GetAngle() * 180 / Math.PI; //in degrees

          if(ang < curMin){

            curMin = ang;
            curMinInt = he.Index;

            _vertMinAngHeInts[v.Index] = curMinInt;
          }
        }
      }

      //conditional 2 methods fot V nodes KINK nodess
      if( _isVVertices[v.Index]){
        foreach(HeMesh3d.Halfedge he in v.OutgoingHalfedges){

          if(_isActiveHalfedges[he.Index] && _isActiveHalfedges[he.Previous.Twin.Index] ){

            _vertMinAngHeInts[v.Index] = he.Index;
          }
        }
      }
    }


  }
  ////////////////////

/// <summary>
        /// //// A MEHOD FOR CALCULATING THE OFFSETS FOR A MINIMAL POLYGON ABOUT A VALENCE 3 y VERTEX
        /// </summary>

  void SetAngleDistsGeneral(){

    double EE;

    ///NOTE THIS IS ACCESSED IN SORTED ORDER FROM VERTEX ACCESSING OUTGOING HALFEDGES ONLY
    for (int i = 0; i < _activeVertexSelectionInts.Count; i++){


      //if it is a y do the stuff
      int curMin = _vertMinAngHeInts[_activeVertexSelectionInts[i]];
      HeMesh3d.Halfedge he = _hemesh.Halfedges[curMin];

      //get base tringle see sketch trig

      double a = he.GetAngle() * 180 / Math.PI; //in degrees

      double b = (180 - (180 - a)) / 2;

      double AA = Math.Sin(a * (Math.PI / 180)) * ( _plateDepth / Math.Sin(b * (Math.PI / 180)));

      double e = 90 - b;

      EE = Math.Sin(e * (Math.PI / 180)) * ( AA / Math.Sin(he.GetAngle()));

      /////////////// the first and previois outgoing  edges are assigned
      //assign the angular distace value for the current node angle

      _angularHeDists[he.Index] = EE;
      _angularHeDists[he.Previous.Twin.Index] = EE;

      //////do the trig stuff to get other halfedge distance assignment
      double twinnextang = he.Twin.Next.GetAngle() * 180 / Math.PI; //in degrees
      double r = 360 - 90 - 90 - twinnextang;  //in degrees
      double p = r / 2; //in degrees
      double t = p; //in degrees

      //the notation here is per the trig drawing in sketchbook if you dont have it goodluck to you....
      double twinmindirlen = EE;
      double NN = (twinmindirlen / Math.Sin(p * (Math.PI / 180))) * Math.Sin(90 * (Math.PI / 180));
      double TT = (NN / Math.Sin(90 * (Math.PI / 180))) * Math.Sin(t * (Math.PI / 180));


      _angularHeDists[he.Twin.Next.Index] = TT;
      //}


    }


  }

  /// <summary>
        /// //// A MEHOD FOR CALCULATING THE OFFSETS FOR A MINIMAL POLYGON ABOUT A VALENCE 2 V kink vertex
        /// </summary>
  void SetAngleDistsStandardV(){

    //double EE;

    ///NOTE THIS IS ACCESSED IN SORTED ORDER FROM VERTEX ACCESSING OUTGOING HALFEDGES ONLY
    for (int i = 0; i < _activeVertexSelectionInts.Count; i++){
      //apply conditional mask
      if(_isStandardVVertices[_activeVertexSelectionInts[i]]){

        int curMin = _vertMinAngHeInts[_activeVertexSelectionInts[i]];
        HeMesh3d.Halfedge he = _hemesh.Halfedges[curMin];



        double bDEG = (_standardVangle / 2); //in degrees
        double b = bDEG * Math.PI / 180; //in radians
        double aDEG = 180 - 90 - bDEG; //in degrees
        double a = aDEG * Math.PI / 180; //in radians

        double AA = Math.Abs((_plateDepth / Math.Sin(b)) * Math.Sin(a));


        //set the first two do filtering to make sure it should receive data
        if(_isActiveHalfedges[he.Index]){
          _angularHeDists[he.Index] = AA;
        }

        if(_isActiveHalfedges[he.Previous.Twin.Index]){
          _angularHeDists[he.Previous.Twin.Index] = AA;
        }

        //the last angular distance is a null in set so add it as zero---HAHAHAH it is needed to box in the triagle analtyically...cant solve in fiorce particle wokrs


        //////do the trig stuff to get other halfedge distance assignment
        double twinnextang = he.Twin.Next.GetAngle() * 180 / Math.PI; //in degrees
        double r = 360 - 90 - 90 - twinnextang;  //in degrees
        double p = r / 2; //in degrees
        double t = p; //in degrees

        //the notation here is per the trig drawing in sketchbook if you dont have it goodluck to you....
        double twinmindirlen = _angularHeDists[he.Previous.Twin.Index];
        double NN = (twinmindirlen / Math.Sin(p * (Math.PI / 180))) * Math.Sin(90 * (Math.PI / 180));
        double TT = (NN / Math.Sin(90 * (Math.PI / 180))) * Math.Sin(t * (Math.PI / 180));


        //do a mask to ensure inhgeritance is not f'd


        //assign the angular distace value for the remainer edge in outgoing node angle

        _angularHeDists[he.Twin.Next.Index] = TT;





      }
    }
  }

  ////////////////////


  ////////////////////