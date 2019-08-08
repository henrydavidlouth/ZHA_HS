HS_INITS.cs
// a class containing the initialisation functions 
// 1. for massing scale deployment
/

 /////VARS
  HeMesh3d _hemesh;

  IList<Plane> _hePlanes;

  double _winDist;
  double _plateDepth;

////////////////////

void InitMesh(HeMesh3d hm)
  {
    _hemesh = new HeMesh3d(hm);
  }
  //////////////////////

 		/// <summary>
        /// Sets the user defined variables
        /// </summary>
  void  InitFootprintConstraints(double winDist, double plateDepth){
    _plateDepth = plateDepth;
    _winDist = winDist;
  }
  ////////////////////

   /// <summary>
        /// Initialises the variabel and containers for variable derived from other inputs; process data 
        /// </summary>
  void InitDerivedVariables(){

    _angularHeDists = new List <double> ();
    _windowHeDists = new List <double> ();

    //load dummy info--should probably use an array
    foreach (HeMesh3d.Halfedge he in _hemesh.Halfedges){

      _angularHeDists.Add(0);
      _windowHeDists.Add(0);

    }

  }

 
  /// <summary>
        /// initialises and sets the extracted vectoral values from the hemesh; 
  ///separate buffer bins for stroing additioanl attributes needed from the halfedge
        /// </summary>

  /////NOTE THESE LISTS RUN PARALLEL TO THE INDEXING OF THE HALFEDGE LIST
  void SetMeshVariables(){
    _hePlanes = new List <Plane> ();
    _axialHeDirs = new List <Vec3d> ();
    _offsettingHeDirs = new List <Vec3d> ();

    foreach (HeMesh3d.Halfedge he in _hemesh.Halfedges){
      Vec3d v0 = he.Start.Position;
      Vec3d v1 = he.End.Position; //xheading
      Vec3d v2 = he.Previous.Start.Position; //y heading

      Point3d pt0 = new Point3d(v0.X, v0.Y, v0.Z);
      Point3d pt1 = new Point3d(v1.X, v1.Y, v1.Z);
      Point3d pt2 = new Point3d(v2.X, v2.Y, v2.Z);

      Plane pl = new Plane(pt0, pt1, pt2);

      _hePlanes.Add(pl);


      //g et the derivatives to remove conversion downstream it is lame but saves verbose elsewhere
      Rhino.Geometry.Vector3d vx = pl.XAxis;
      Rhino.Geometry.Vector3d vy = pl.YAxis;

      //convert to spatialslur namespace
      Vec3d vxss = new Vec3d(vx.X, vx.Y, vx.Z);
      Vec3d vyss = new Vec3d(vy.X, vy.Y, vy.Z);

      //assign to the array
      _axialHeDirs.Add(vxss);
      _offsettingHeDirs.Add(vyss);

    }
  }
  ///////////////////////

///////////////////////
  void SetActiveOutgoingHeNeighbors2(){
    _isActiveHalfedges = new List<bool> ();

    //load with dummy info
    for (int i = 0; i < _hemesh.Halfedges.Count; i++){
      _isActiveHalfedges.Add(false);
    }

    //load withactual
    for (int i = 0; i < _hemesh.Halfedges.Count; i++){

      if(_fLengths[i] > 0){

        _isActiveHalfedges[i] = true;
      }
    }



  }
  ////////////////////




  ///////////////////////