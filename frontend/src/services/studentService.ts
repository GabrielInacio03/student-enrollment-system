import axios from 'axios'
import type { Student } from '../types/Student'

const API_URL = 'http://localhost:5000/api/students' // ajuste conforme sua porta/backend

export async function getAllStudents(): Promise<Student[]> {
  const response = await axios.get(API_URL)
  return response.data
}

export async function saveStudent(student: Student): Promise<void> {
  if (student.id) {
    await axios.put(`${API_URL}/${student.id}`, student)
  } else {
    await axios.post(API_URL, student)
  }
}

export async function deleteStudentById(id: number): Promise<void> {
  await axios.delete(`${API_URL}/${id}`)
}