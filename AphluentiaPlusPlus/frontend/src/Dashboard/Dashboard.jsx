import React from 'react';
import {useContext, useState, useEffect} from 'react';
import axios from 'axios';
import ModuleList from  './ModuleList.jsx';
import './Dashboard.css';
import SearchBar from  '../Base/SearchBar.jsx';
import AddModule from './AddModule.jsx';
import Loading from '../Base/Loading.jsx';

const DashboardPage = () => {
    const userType = localStorage.getItem("UserType");
    const token = localStorage.getItem("Token");
    const [isLoading, setLoading] = useState(false);
    const [patientList, setPatientList] = useState([]);

    let baseUrl = "https://localhost:7176/api/"+(userType == 0 ?  "Patient/"+token : "Therapist/"+token+"/Patients" )
    const getTherapistData = () => {
        axios
            .get(baseUrl+"/All")
            .then(data => {
                setPatientList(data.data.data);
                setLoading(false);
            })
            .catch(error => {
                // Handle error
        });
    }
    const getPatientsInfo = () => {
        console.log(userType)
        userType == 0 ? 
            axios
                .get(baseUrl)
                .then(data => {
                    setPatientList(data.data.data);
                    setLoading(false);
                })
                .catch(error => {
                    console.log(error)
                })
            : 
            getTherapistData()
        
    }
    useEffect(() => {
        setLoading(true);
        getPatientsInfo();
    }, []);



    const [list, setList] = useState(patientList);
    const [query, setQuery] = useState('');
    if (patientList != list && query ==='' ){
      setList(patientList);
    }
  
    const handleInputChange = (e) => {
      const inputValue = e.target.value;
      setQuery(inputValue);
      let filteredList = patientList.filter(item =>
        item.email.toLowerCase().includes(inputValue.toLowerCase()) ||
        item.firstName.toLowerCase().includes(inputValue.toLowerCase()) ||
        item.lastName.toLowerCase().includes(inputValue.toLowerCase()) || 
        (item.firstName + " " + item.lastName).toLowerCase().includes(inputValue.toLowerCase()) ||
        item.conditionName.toLowerCase().includes(inputValue.toLowerCase())
      );
      if (inputValue === '') {
       filteredList = patientList;
      }
      setList(filteredList);
    };
  
    
    return isLoading ? 
            <Loading/>
            :                
            <>

                <div className="patient-container">
                    <h1>Patient's Modules</h1>
                        
                    
                    {userType == 0 ? 
                        <div className="patient-row">
                        
                            <div className="patient-info">
                                <h3>{patientList.firstName} {patientList.lastName}</h3>
                                <p>{patientList.email}</p>
                                <AddModule></AddModule>

                            </div>
                            <ModuleList key={patientList.email} patientEmail={patientList.email}/>
                        </div>:
                        <> 
                            <SearchBar placeholder="Search Patients..." onChange={(e) => {handleInputChange(e)}} />
                            {list.map((patient, index) => {
                                return  <div key={patient.email}  className="patient-row">
                                            <div className="patient-info">
                                                <h3>{patient.firstName} {patient.lastName}</h3>
                                                <p>{patient.email}</p>
                                                <p>{patient.conditionName}</p>
                                            </div>
                                            <ModuleList key={patient.email} patientEmail={patient.email}/>
                                        </div>

                            })}
                        </>
                    }
                </div>
            </>
    };

export default DashboardPage;
