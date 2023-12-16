import React from 'react';
import {useContext, useState, useEffect} from 'react';
import axios from 'axios';
import './ModuleList.css';
import ModuleCard from './ModuleCard';
import SearchBar from  '../Base/SearchBar.jsx';
import Loading from '../Base/Loading.jsx';

const ModuleList = ({patientEmail}) => {
    const userType = localStorage.getItem("UserType");
    const token = localStorage.getItem("Token");
    const [moduleList, setModuleList] = useState([]);
    const [loading, setLoading] = useState(false);
  
    let baseUrl = "https://localhost:7176/api/"+(userType == 0 ?  "Patient/"+token+"/Modules" : "Therapist/"+token+"/Patients/"+patientEmail+"/Modules" )
    const getModules = () => {
        setLoading(true);
        axios
            .get(baseUrl)
            .then(data => {
                setModuleList(data.data.data);
                setLoading(false);
            })
            .catch(error => {
                // Handle error
            });
    }
    const deleteModule= (url) =>{
        if (userType == 0){
            axios
               .delete(baseUrl)
               .then(data => {
                   
               })
               .catch(error => {
                   // Handle error
               });
       }
      
    }
    useEffect(() => {
        getModules();
    }, []);

    const [list, setList] = useState(moduleList);
    const [query, setQuery] = useState('');
    if (moduleList != list && query ==='' ){
      setList(moduleList);
    }
  
    const handleInputChange = (e) => {
      const inputValue = e.target.value;
      setQuery(inputValue);
      let filteredList = moduleList.filter(item =>
        item.id.toLowerCase().includes(inputValue.toLowerCase()) ||
        item.moduleData.applicationName.toLowerCase().includes(inputValue.toLowerCase()) ||
        item.moduleData.versionId.toLowerCase().includes(inputValue.toLowerCase())
      );
      if (inputValue === '') {
       filteredList = moduleList;
      }
      setList(filteredList);
    };


    return (loading == true ? 
            <Loading/>
            :
            <>
                <SearchBar placeholder="Search Modules..." onChange={(e) => {handleInputChange(e)}} />
                <div className="modules-row">
                    {list.map((module,index) =>{ return <ModuleCard key={module['id']} deleteModule={deleteModule} ModuleId={module['id']} PatientEmail={patientEmail}/>})}
                </div>
            </>
        
    );
};

export default ModuleList;
