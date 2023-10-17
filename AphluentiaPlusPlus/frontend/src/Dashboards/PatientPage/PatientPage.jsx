import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './TherapistPage.css';
import PatientProfile from './PatientProfile';
import TherapistRequests from './TherapistSectionComponents/TherapistRequests';
import PendingTherapists from './TherapistSectionComponents/PendingTherapists';
import TherapistList from './TherapistSectionComponents/TherapistList';
import AcceptedTherapist from './TherapistSectionComponents/AcceptedTherapist';

const PatientPage = () => {
  const [AcceptedTherapistList, setAcceptedTherapist] = useState([]);
  const [RequestedTherapists, setRequestedTherapist] = useState([]);
  const [AvailableTherapists, setAvailableTherapist] = useState([]);
  const [PendingTherapist, setPendingTherapists] = useState([]);
  const [section, setSection] = useState(1);
  const token = localStorage.getItem('Authentication');

  useEffect(() => {
    const email = localStorage.getItem('Email');
    const intervalId = setInterval(() => {
      getPatientTherapists();
    }, 1000);
    return () => clearInterval(intervalId);
  }, []);
  const getPatientTherapists = () =>{
    axios
    .get("https://localhost:7176/api/Patient/"+ token+"/Therapists")
    .then(data =>{
      setAcceptedTherapist(data.data.data.accepted);
      setRequestedTherapist(data.data.data.requested);
      setAvailableTherapist(data.data.data.available);
      setPendingTherapists(data.data.data.pending);
      navigate("/Dashboard");

    } )
    .catch(error => {
    });

  }
  console.log(token);
  const handleTherapistRequest = (email) => {
    console.log(email);
    axios
    .get("https://localhost:7176/api/Patient/" + token + "/" + email)
    .then(data => {
      getPatientTherapists();
    })
    .catch(error => {
      // Handle error
    });
  };
  const handleTherapistReject = (email) => {
    axios
    .delete("https://localhost:7176/api/Patient/" + token + "/" + email)
    .then(data => {
      
      getPatientTherapists();
    })
    .catch(error => {
      // Handle error
    });
  };
  const handleTabChange = (tabNumber) => {
    setSection(tabNumber);
  };
 

  return (
    <div>
     {RequestedTherapists == null ||AvailableTherapists == null ||PendingTherapist == null   ? (
                  null
          ) : (
            <div className="container">
              <div className="content-container" >
                <PatientProfile therapist={AcceptedTherapist} handleTherapistReject = {handleTherapistReject}/>
                <AcceptedTherapist therapists={AcceptedTherapistList} handleTherapistReject = {handleTherapistReject}/>
              </div>
            <div className="tab-container">
              <div className="buttons-container">
                <button
                  className={section === 0 ? 'active' : 'disabled'}
                  onClick={() => handleTabChange(0)}
                >
                  Applications
                </button>
                <button
                  className={section === 1 ? 'active' : 'disabled'}
                  onClick={() => handleTabChange(1)}
                >
                  Therapist Settings
                </button>
              </div>
            </div>
            <div className="content-container">
             
                {section === 0 ? 
                  <div className="content-container-sections">
                   
                  </div>
                  :
                  <div className="content-container-sections">
                    <TherapistRequests therapists={RequestedTherapists} handleTherapistReject = {handleTherapistReject} handleTherapistAccept = {handleTherapistRequest}/>
                    <PendingTherapists therapists={PendingTherapist} handleTherapistReject = {handleTherapistReject}/>
                    <TherapistList therapists={AvailableTherapists} handleTherapistRequest = {handleTherapistRequest} />
                  </div>
                }
            </div>
          </div>
          )}
                      
    </div>
  );
};

export default PatientPage;
