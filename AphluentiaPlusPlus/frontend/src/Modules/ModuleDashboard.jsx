import React from 'react';
import {useContext, useState, useEffect} from 'react';
import axios from 'axios';
import { json, useNavigate, useParams } from 'react-router-dom';

const ModuleDashboardPage = () => {
    const navigate = useNavigate();
    const userType = localStorage.getItem("UserType");
    const token = localStorage.getItem("Token");
    const [module, setModule] = useState(null);
    const {moduleId, patient} = useParams();
    let baseUrl = '';
 
    
  
    const getModuleInformation = () => {
        axios
            .get(baseUrl+"/Modules/"+moduleId)
            .then(data => {
                console.log(data.data.data)
                setModule(data.data.data);
            })
            .catch(error => {
                // Handle error
            })
        
    }
    useEffect(() => {
        console.log(moduleId)
        if (moduleId == undefined){
            //alert("No Module Data Provided");
            //return navigate("/Home");
        }
        if (userType == 1 && patient == undefined){
            //alert("Module Not Found"); 
            //return navigate("/Home");
        }
    
        baseUrl = "https://localhost:7176/api/"+(userType == 0 ?  "Patient/"+token : "Therapist/"+token+"/Patients/"+patient )
        getModuleInformation();
    }, []);



    return (
        module == null ? <></>:<>
             <div>
               <h1>{module.moduleData.applicationName}</h1>
               <p>{module.moduleData.versionId}</p>
               <div dangerouslySetInnerHTML={{ __html: module.moduleData.htmlDashboard }} >
               </div>
               <div >
                {JSON.stringify(module.moduleData.dataStructure)}
               </div>
            </div>
        </>
        )
    };

export default ModuleDashboardPage;
