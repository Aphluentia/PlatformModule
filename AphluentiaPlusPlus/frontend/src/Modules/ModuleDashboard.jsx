import React from 'react';
import {useContext, useState, useEffect} from 'react';
import axios from 'axios';
import { json, useNavigate, useParams } from 'react-router-dom';
import './ModuleDashboard.css';
import ModuleJSONData from './ModuleJSONData';
import ModuleHTML from './ModuleHTML';

const ModuleDashboardPage = () => {
    const navigate = useNavigate();
    const userType = localStorage.getItem("UserType");
    const token = localStorage.getItem("Token");
    const [module, setModule] = useState(null);
    const {moduleId, patient} = useParams();
    let baseUrl = '';
 
    
  
    const getModuleInformation = () => {
        console.log("Hello???")
        baseUrl = "https://localhost:7176/api/"+(userType == 0 ?  "Patient/"+token : "Therapist/"+token+"/Patients/"+patient )
        axios
            .get(baseUrl+"/Modules/"+moduleId)
            .then(data => {
                setModule(data.data.data);
            })
            .catch(error => {
                // Handle error
            })
        
    }  
    const updateModuleInformation = () => {
        baseUrl = "https://localhost:7176/api/"+(userType == 0 ?  "Patient/"+token : "Therapist/"+token+"/Patients/"+patient )
        axios
            .put(baseUrl+"/Modules/"+moduleId, module)
            .then(data => {
                alert("Updated");
                getModuleInformation();
            })
            .catch(error => {
                // Handle error
            })
        
    }
  
    useEffect(() => {
        if (moduleId == undefined){
            alert("No Module Data Provided");
            return navigate("/Home");
        }
        if (userType == 1 && patient == undefined){
            alert("Module Not Found"); 
            return navigate("/Home");
        }

        getModuleInformation();
  
    
        
    }, []);

    const handleInputChange = (sectionName, content) => {
        let tempModule = module;
        for (let section of tempModule.moduleData.dataStructure) {
            if (section.sectionName === sectionName) {
                section.content = content;
                break;
            }
        }
        setModule(tempModule);
        updateModuleInformation(module)
       
      };
      const handleHtmlChange = (newHtmlCode) => {
        let tempModule = module;
        tempModule.moduleData.htmlDashboard = newHtmlCode;
        setModule(tempModule);
        updateModuleInformation(module)
        
      };
    return (
        module === null 
        ? null 
        : (
            <div className="module-container">
                <h1 className="module-title">{module.moduleData.applicationName}</h1>
                <p className="module-version">{module.moduleData.versionId}</p>
                <div className="module-html-dashboard">
                    <ModuleHTML htmlDashboard={module.moduleData.htmlDashboard} datastructure={module.moduleData.dataStructure} handleHtmlChange={handleHtmlChange} setModule={setModule}></ModuleHTML>
                </div>
                <div className="module-data-structure">
                    {module.moduleData.dataStructure.map((datapoint, index) => {
                        return (
                            <ModuleJSONData 
                                key={index} 
                                initialDatapoint={datapoint} 
                                handleDataChange={handleInputChange} 
                                setModule={setModule}
                            />
                        );
                    })}
                </div>
            </div>
        )
        
        )
    };

export default ModuleDashboardPage;
