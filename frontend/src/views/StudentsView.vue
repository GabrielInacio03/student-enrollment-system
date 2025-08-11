<template>
  <div>
    <h2>Listagem de Alunos</h2>
    <input v-model="search" placeholder="Buscar aluno..." />
    <button @click="openForm()">Cadastrar Aluno</button>

    <table>
      <thead>
        <tr>
          <th>RA</th>
          <th>Nome</th>
          <th>CPF</th>
          <th>Ações</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="student in filteredStudents" :key="student.id">
          <td>{{ student.ra }}</td>
          <td>{{ student.name }}</td>
          <td>{{ student.cpf }}</td>
          <td>
            <button @click="editStudent(student)">Editar</button>
            <button @click="deleteStudent(student.id)">Excluir</button>
          </td>
        </tr>
      </tbody>
    </table>

    <StudentForm
      v-if="showForm"
      :student="selectedStudent"
      @close="closeForm"
      @saved="refreshList"
    />
  </div>
</template>

<script lang="ts" setup>
import { ref, computed, onMounted } from 'vue'
import StudentForm from '../components/StudentForm.vue'
import { getAllStudents, deleteStudentById } from '../services/studentService'
import type { Student } from '../types/Student'

const students = ref<Student[]>([])
const search = ref('')
const showForm = ref(false)
const selectedStudent = ref<Student | null>(null)

const filteredStudents = computed(() =>
  students.value.filter(s =>
    s.name.toLowerCase().includes(search.value.toLowerCase())
  )
)

function openForm(student: Student | null = null) {
  selectedStudent.value = student
  showForm.value = true
}

function closeForm() {
  selectedStudent.value = null
  showForm.value = false
}

async function deleteStudent(id: number) {
  await deleteStudentById(id)
  await refreshList()
}

async function refreshList() {
  students.value = await getAllStudents()
}

onMounted(refreshList)
</script>