
import './App.css'
import Navbar from './Base/Navbar.jsx'
import { BrowserRouter, Route, Routes} from "react-router-dom";
import Modules from './Modules/Modules.jsx';
import Dashboard from './Dashboard/Dashboard.jsx';
import Login from './Login/Login.jsx';
import {Context, SessionData} from './Context'
import {useContext, useState} from 'react';

import Home from './Home/Home';
function App() {

  const context = useContext(Context);
  const [WebPlatformId, setWebPlatformId] = useState(context);

  
  const session = useContext(SessionData);
  const [sessionData, setSessionData] = useState(session);
  
  const [error, setError] = useState('');
  const [inputValue, setInputValue] = useState('');
  
  
  const handleChange = (event) => {
    setInputValue(event.target.value);
  };

  const handleGenerateSession = () => {
      setWebPlatformId(inputValue);
      fetch(`https://localhost:7176/api/services/GenerateSession?WebPlatformId=${inputValue}`)
          .then(response => response.json())
          .then(data => setSessionData(data))
          .catch(error => setError(error));
  };
 
  return (<Context.Provider value={[WebPlatformId, setWebPlatformId]}>
    <SessionData.Provider value={[sessionData, setSessionData]}>
      <BrowserRouter>
          <Navbar sessionData={sessionData}/>
          <div className="AppBody"> 
            <Routes>
              <Route path="/" element={<Home/>} />
              <Route path="/login" element={<Login/>} />
              <Route path="/modules" element={<Modules sessionData={sessionData}/>} />
              <Route path="/dashboard" element={<Dashboard/>} />
            </Routes>
          </div>
         


          <div className='DevInput'>
              <input type="text" onChange={handleChange} />
              <button onClick={handleGenerateSession}>Generate Session</button>
              {sessionData===null ? <p>Loading</p>: <p>{JSON.stringify(sessionData)}</p>}
          </div>

            
         
        </BrowserRouter>
        </SessionData.Provider>
  </Context.Provider>)
    

    
  
}

export default App
