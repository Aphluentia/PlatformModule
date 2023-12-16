import React from 'react';
import {useContext, useState, useEffect} from 'react';
import axios from 'axios';
import './ModuleCard.css';
import { useNavigate } from 'react-router-dom';


const ModuleCard = ({ModuleId, PatientEmail, deleteModule}) => {
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
    const removeModuleCard = () => {
        deleteModule(baseUrl);
      
    }
    const handleRedirect = () => {
        navigate("/modules/" + ModuleId + "/" + PatientEmail);
    };
    useEffect(() => {
        getModules();
    }, []);

    const replaceCardPlaceholders = (templateString) => {
        return templateString.replace(/{{(.*?)}}/g, (match, placeholder) => {
            return getJsonDirective(placeholder.split('.'), module);  
        });
    };
    const getJsonDirective=(keys, section)=>{
        if (keys.length == 0) return section;
        else if (keys.length == 1) return section[keys[0]];
        else{
           return getJsonDirective(keys.slice(1), section[keys[0]])
        }
       
    }
 
    return (<div className="module-card">
            <a onClick={handleRedirect}> 
            
                {module !== ""? <div dangerouslySetInnerHTML={{ __html: replaceCardPlaceholders(module.moduleData.htmlCard) }} />:<><p>Loading...</p></>} 
            </a>
            {userType == 0 ? (
                <button className="button-remove" onClick={() => removeModuleCard()}>
                    Remove
                </button>
            ) : <></> }

           
        </div>
        
    );
};

export default ModuleCard;
