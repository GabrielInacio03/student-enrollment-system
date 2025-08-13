import axios from 'axios'
import type { Student } from '../types/Student'

const API_URL = 'http://localhost:5120/api/students' 

export async function getAllStudents(): Promise<Student[]> {
  const response = await axios.get(API_URL)
  return response.data
}
export async function getStudentsPaged(page: number, pageSize: number = 10, search: string = '') {
  const response = await axios.get(API_URL+'/paged', {
    params: { page, pageSize, search }
  })
  return response.data
}
export async function getStudentById(id: number): Promise<Student> {
  const response = await axios.get(`${API_URL}/${id}`)
  return response.data
}
export async function createStudent(student: Student): Promise<void> {
  await axios.post(API_URL, student)
}
export async function updateStudent(id: number, student: Student): Promise<void> {
  await axios.put(`${API_URL}/${id}`, student)
}
export async function deleteStudentById(id: number): Promise<void> {
  await axios.delete(`${API_URL}/${id}`)
}