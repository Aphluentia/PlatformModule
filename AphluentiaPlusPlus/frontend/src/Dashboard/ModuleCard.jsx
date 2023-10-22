import React from 'react';
import {useContext, useState, useEffect} from 'react';
import axios from 'axios';
import './ModuleCard.css';
import { useNavigate } from 'react-router-dom';


const ModuleCard = ({ModuleId, PatientEmail}) => {
    const userType = localStorage.getItem("UserType");
    const token = localStorage.getItem("Token");
    const [module, setModule] = useState("");
    const navigate = useNavigate();

    let baseUrl = "https://localhost:7176/api/"+(userType == 0 ?  "Patient/"+token+"/Modules/"+ ModuleId : "Therapist/"+token+"/Patients/"+PatientEmail+"/Modules/"+ ModuleId )
    const getModules = () => {
        axios
        .get(baseUrl)
        .then(data => {
            setModule(data.data.data);
        })
        .catch(error => {
            // Handle error
        });
    }
    const handleRedirect = () => {
        navigate("/modules/" + ModuleId + "/" + PatientEmail);
    };
    useEffect(() => {
        getModules();
    }, []);
 
    return (<div className="module-card">
            <a onClick={handleRedirect}> 
                {module !== ""? <div dangerouslySetInnerHTML={{ __html: module.moduleData.htmlCard }} />:<><p>Loading...</p></>} 
            </a>
           
        </div>
        
    );
};

export default ModuleCard;
