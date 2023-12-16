import React from 'react';
import {useContext, useState, useEffect} from 'react';
import axios from 'axios';
import { json, useNavigate, useParams } from 'react-router-dom';
import './ModuleDashboard.css';
import ModuleHTML from './ModuleHTML';
import ModuleJSONDataList from './ModuleJSONDataList.jsx';
import ModuleDetails from './ModuleDetails.jsx';
import ModuleProfiles from './ModuleProfiles.jsx';
import { InfinitySpin } from  'react-loader-spinner'

const ModuleDashboardPage = () => {
    const navigate = useNavigate();
    const userType = localStorage.getItem("UserType");
    const token = localStorage.getItem("Token");
    const [module, setModule] = useState(null);
    const {moduleId, patient} = useParams();
    const [availableProfiles, setAvailableProfiles] = useState([])
    let baseUrl = '';
 
    const createProfile = (profileName) => {
        baseUrl = "https://localhost:7176/api/"+(userType == 0 ?  "Patient/"+token : "Therapist/"+token+"/Patients/"+patient )
        axios
            .post(baseUrl+"/Modules/"+moduleId+"/Profile/"+profileName, "")
            .then(data => {
                getModuleInformation();
            })
            .catch(error => {
                // Handle error
            })
        
    }
    const deleteProfile = (profileName) => {
        baseUrl = "https://localhost:7176/api/"+(userType == 0 ?  "Patient/"+token : "Therapist/"+token+"/Patients/"+patient )
        axios
            .delete(baseUrl+"/Modules/"+moduleId+"/Profile/"+profileName)
            .then(data => {
                getModuleInformation();
            })
            .catch(error => {
                console.log(error);
                // Handle error
            })
        
    }
    const updateCurrentProfile = (profileName) => {
        module.moduleData.activeContextName = profileName;
        setModule(module);
        updateModuleInformation(module)
    }
  
    const getModuleInformation = () => {
        baseUrl = "https://localhost:7176/api/"+(userType == 0 ?  "Patient/"+token : "Therapist/"+token+"/Patients/"+patient )
        axios
            .get(baseUrl+"/Modules/"+moduleId)
            .then(data => {
                setModule(data.data.data);
                let temp = []
                for (let datapoint of data.data.data.moduleData.dataStructure){
                    temp.push(datapoint.contextName);
                }  
                setAvailableProfiles(temp);
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
   
    const updateModuleToVersion = (versionId) => {
        baseUrl = "https://localhost:7176/api/"+(userType == 0 ?  "Patient/"+token : "Therapist/"+token+"/Patients/"+patient )
        axios
            .put(baseUrl+"/Modules/"+moduleId+"/Version/"+versionId, "")
            .then(data => {
                alert("Updated To Version");
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
            console.log(section);
            if (section.sectionName === sectionName && section.contextName == module.moduleData.activeContextName) {
                section.content = content;
                break;
            }
        }
        setModule(tempModule);
        updateModuleInformation(module)
       
      };
      const handleHtmlChange = (newHtmlCode, card) => {
        let tempModule = module;
        if (card){
            tempModule.moduleData.htmlCard = newHtmlCode;
        }else{
            tempModule.moduleData.htmlDashboard = newHtmlCode;
        }
        setModule(tempModule);
        updateModuleInformation(module)
        
      };
    return (
        module === null 
        ? 
        <InfinitySpin 
            width='200'
            color="#4fa94d"
        /> 
        : (
            <div className="module-container">
                <ModuleDetails module={module} updateToVersion={updateModuleToVersion}/>
                <div className="module-inner-container">
                    <ModuleProfiles updateCurrentProfile={updateCurrentProfile} profilesList={availableProfiles} currentProfile={module.moduleData.activeContextName} createProfile={createProfile} deleteProfile={deleteProfile}/>
                    <ModuleJSONDataList module={module} handleDataChange={handleInputChange} setModule={setModule} />
                </div>
                <div className="module-inner-container">
                    <ModuleHTML module={module} datastructure={module.moduleData.dataStructure} handleHtmlChange={handleHtmlChange} setModule={setModule}></ModuleHTML>
                </div>
            </div>
        )
        
        )
    };

export default ModuleDashboardPage;
