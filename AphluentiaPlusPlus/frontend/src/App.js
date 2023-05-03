import React, {useEffect, useState} from 'react';
import Modules from "./Modules/Modules";


function App() {
  const [sessionData, setSessionData] = useState(null);
  var webPlatformId = "AphluentiaPlusPlus"
  useEffect(() => {
    const generateSession = async () => {
      var response = await fetch(`https://localhost:7176/api/Services/GenerateSession?WebPlatformId=${webPlatformId}`);
      var jsonified = await response.json();
      setSessionData(jsonified.data);
    };
    generateSession();
  },[setSessionData])
 
  return (<>
      {sessionData && <Modules sessionData={sessionData} />}
  </>)
}

export default App;
