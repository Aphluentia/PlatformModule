import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import './index.css'
import Navbar from './Base/Navbar.jsx';
import { BrowserRouter, Route, Routes} from "react-router-dom";
import Modules from './Modules/Modules.jsx';
import Dashboard from './Dashboard/Dashboard.jsx';
import Login from './Login/Login.jsx';





ReactDOM.createRoot(document.getElementById('root')).render(
      
      <React.StrictMode>
        <App />
        
       
      </React.StrictMode>
)
