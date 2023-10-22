import React from 'react';
import {useContext, useState, useEffect} from 'react';
import axios from 'axios';
import ModuleCard from './ModuleCard';
import SearchBar from  '../Base/SearchBar.jsx';

const ModuleList = ({patientEmail}) => {
    const userType = localStorage.getItem("UserType");
    const token = localStorage.getItem("Token");
    const [moduleList, setModuleList] = useState([]);
  
    let baseUrl = "https://localhost:7176/api/"+(userType == 0 ?  "Patient/"+token+"/Modules" : "Therapist/"+token+"/Patients/"+patientEmail+"/Modules" )
    const getModules = () => {
        axios
        .get(baseUrl)
        .then(data => {
            setModuleList(data.data.data);
        })
        .catch(error => {
            // Handle error
        });
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


    return (<>
                <div className="modules-row">
                    <SearchBar placeholder="Search Modules..." onChange={(e) => {handleInputChange(e)}} />
                    {list.map((module,index) =>{ return <ModuleCard key={module['id']} ModuleId={module['id']} PatientEmail={patientEmail}/>})}
                </div>
            </>
        
    );
};

export default ModuleList;
